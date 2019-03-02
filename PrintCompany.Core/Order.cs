using System;
using System.Collections.Generic;
using System.Text;

namespace PrintCompany.Core
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DueDate { get; set; }
        //public bool PrintRequired { get; set; }
        //public bool EmbroideryRequired { get; set; }
        

        public Customer Customer { get; set; }
        public virtual IList<OrderLine> OrderLines { get; set; }
    }
}
