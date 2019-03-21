using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrintCompany.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintCompany.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int PrintQuantityTotalByOrder { get; set; }
        public int EmbroideryQuantityTotalByOrder { get; set; }
        public int PrintQuantityCompletedTotalByOrder { get; set; }
        public int EmbroideryQuantityCompletedTotalByOrder { get; set; }

        //public Customer Customer { get; set; }
        public IList<FileUpload> FileUploads { get; set; }
        public List<OrderLineViewModel> orderLineViewModels { get; set; }
        public IList<FileUpload> Files { get; set; }

    }
}
