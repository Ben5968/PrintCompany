using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintCompany.Core;
using PrintCompany.Data;

namespace PrintCompany.Controllers
{

    public class FileUploadsController : Controller
    {
        private readonly PrintCompanyDbContext _context;
        public IHostingEnvironment Host { get; }

        public FileUploadsController(PrintCompanyDbContext context, IHostingEnvironment host)
        {
            Host = host;
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

            var uploadsFolderPath = Path.Combine(Host.WebRootPath, "content/uploads");
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
                OriginalFileName = file.FileName
            };

            order.Files.Add(newwFile);
            //await _context.SaveChangesAsync();
            _context.SaveChanges();

            return Json(new { status = true, Message = "Account created." });
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        //    public ActionResult GetAttachments(int Id)
        //    {
        //        //Get the files list from repository
        //        var orderFiles = _context.FileUploads.Where(x => x.OrderId == Id).ToList();

        //        //string webRootPath = Host.WebRootPath;
        //        //string contentRootPath = Host.ContentRootPath;
        //        //var uploadsFolderPath = Path.Combine(webRootPath, "content/uploads/");

        //        var attachmentsList = from s in orderFiles
        //                              select new
        //                              {
        //                                  FileName = s.OriginalFileName                                     
        //                              };

        //        return Json(new { Data = attachmentsList } );
        //    }
        //}
    }
}