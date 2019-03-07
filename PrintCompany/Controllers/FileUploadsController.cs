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
        public async Task<IActionResult> Upload(int Id, IFormFile files)
        {
            var order = await _context.Orders.FindAsync(Id);
            if (order == null)
            {
                return NotFound();
            }

            var uploadsFolderPath = Path.Combine(Host.WebRootPath, "content/uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var filename = Guid.NewGuid().ToString() + Path.GetExtension(files.FileName);
            var filePath = Path.Combine(uploadsFolderPath, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await files.CopyToAsync(stream);
            }

            var newwFile = new FileUpload
            {
                FileName = filename,
                OriginalFileName = files.FileName
            };

            order.Files.Add(newwFile);
            await _context.SaveChangesAsync();

            return View();
        }
    }
}