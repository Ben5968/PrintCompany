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

        //public Customer Customer { get; set; }
        public IList<FileUpload> FileUploads { get; set; }
        public List<OrderLineViewModel> orderLineViewModels { get; set; }
        public IList<FileUpload> Files { get; set; }
        //public IEnumerable<SelectListItem> ItemTypes { get; set; }
        //public IEnumerable<SelectListItem> ItemSizes { get; set; }
        //public IEnumerable<SelectListItem> ItemColors { get; set; }



    }
}
