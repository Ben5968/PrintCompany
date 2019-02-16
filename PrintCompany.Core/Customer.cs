using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintCompany.Core
{
    //public class Customer : BaseEntity
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string MainContact { get; set; }

        
    }
}
