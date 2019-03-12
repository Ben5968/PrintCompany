using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintCompany.Core
{
    public class FileUpload
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string FileExtension { get; set; }
        
        public int OrderId { get; set; }

        public Order Order { get; set; }        

    }
    
}
