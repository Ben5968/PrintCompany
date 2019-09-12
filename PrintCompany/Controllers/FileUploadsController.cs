using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PrintCompany.Core;
using PrintCompany.Data;

namespace PrintCompany.Controllers
{

    public class FileUploadsController : Controller
    {
        private readonly PrintCompanyDbContext _context;
        public IHostingEnvironment _host { get; }

        public FileUploadsController(PrintCompanyDbContext context, IHostingEnvironment host)
        {
            _host = host;
            _context = context;
        }       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(int Id, IFormFile file)  // Id => OrderId
        {
            //var order = await _context.Orders.FindAsync(Id);
            var order = _context.Orders.Find(Id);
            if (order == null)
            {
                return NotFound();
            }

            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "content/uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                //await file.CopyToAsync(stream);
                file.CopyTo(stream);
            }

            var newwFile = new FileUpload
            {
                FileName = filename,
                OriginalFileName = file.FileName,
                FileExtension = Path.GetExtension(file.FileName)
            };

            order.Files.Add(newwFile);
            //await _context.SaveChangesAsync();
            _context.SaveChanges();

            return Json(new { status = true });
        }

        public FileResult Download(string filename)
        {
            var originalFileName = _context.FileUploads.SingleOrDefault(x => x.FileName == filename).OriginalFileName;
            var filePath = _host.WebRootPath + @"\Content\Uploads\" + filename;
            //var fileExists = System.IO.File.Exists(filePath);          
            return PhysicalFile(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, originalFileName);
        }

        public ActionResult DownloadPDF(string filename)
        {
            var originalFileName = _context.FileUploads.SingleOrDefault(x => x.FileName == filename).OriginalFileName;
            var path = _host.WebRootPath + @"\Content\Uploads\" + filename;
            
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            var cd = new ContentDisposition
            {
                FileName = originalFileName,
                Inline = true
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var result = new FileStreamResult(stream, MediaTypeNames.Application.Octet);

            return result;
        }

        public async Task<IActionResult> Delete(int id)
        {
            var filename = _context.FileUploads.SingleOrDefault(x => x.Id == id).FileName;

            var file = await _context.FileUploads.FindAsync(id);
            if (file == null)
                return NotFound();
            _context.FileUploads.Remove(file);
            await _context.SaveChangesAsync();
                        
            var filePath = _host.WebRootPath + @"\Content\Uploads\" + filename;
            
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return Json("Success");
        }
    }
}