using System;
using System.Collections.Generic;
using System.Text;

namespace PrintCompany.Core
{
    public class OrderCustomerContact
    {
        public int Id { get; set; }
        public int ContactTypeId { get; set; }
        public DateTime ContactDate { get; set; }
        public string ContactedBy { get; set; }
        public string Notes { get; set; }
        public int OrderId { get; set; }

        public Order Order { get; set; }
        public ContactType ContactType { get; set; }

    }
}
