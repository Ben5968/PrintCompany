using PrintCompany.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrintCompany.ViewModels
{
    public class OrderCustomerContactViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ContactTypeId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ContactDate { get; set; }
        public string ContactedBy { get; set; }
        public string Notes { get; set; }  
        public ContactType ContactType { get; set; }
    }
}
