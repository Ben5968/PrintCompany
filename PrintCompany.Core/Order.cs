using System;
using System.Collections.Generic;
using System.Text;

namespace PrintCompany.Core
{
    public class Order
    {
        public Order()
        {
            Files = new List<FileUpload>();
            OrderLines = new List<OrderLine>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public virtual IList<FileUpload> Files { get; set; }
        public virtual IList<OrderLine> OrderLines { get; set; }

        public Customer Customer { get; set; }
    }
}
