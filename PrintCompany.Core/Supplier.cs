using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintCompany.Core
{
    //public class Supplier : BaseEntity
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }        
        public string MainContact { get; set; }
        public string MainContactEmail { get; set; }
        public string CompanyWebSite { get; set; }
        public string CompanyEmail { get; set; }       

    }
}
