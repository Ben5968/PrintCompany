using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrintCompany.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }
        public string InvoiceNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }
        public int PrintQuantityTotalByOrder { get; set; }
        public int EmbroideryQuantityTotalByOrder { get; set; }
        public int PrintQuantityCompletedTotalByOrder { get; set; }
        public int EmbroideryQuantityCompletedTotalByOrder { get; set; }
        public int? ContactTypeId { get; set; }
        public string ContactTypeName { get; set; }
        public string OrderStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime? ContactDate { get; set; }

        [Display(Name = "Related order")]
        public bool RelatedOrderExists { get; set; }

        [Display(Name = "Related order No")]
        public string RelatedOrderNote { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ColectionDate { get; set; }        
        public string CollectionNote { get; set; }

        public IList<OrderCustomerContact> OrderCustomerContacts { get; set; }
        //public Customer Customer { get; set; }
        public IList<FileUpload> FileUploads { get; set; }
        public List<OrderLineViewModel> orderLineViewModels { get; set; }
        public IList<FileUpload> Files { get; set; }

    }
}
