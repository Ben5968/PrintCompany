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
            OrderCustomerContacts = new List<OrderCustomerContact>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? ContactDate { get; set; }
        public int? ContactTypeId { get; set; }
        public bool RelatedOrderExists { get; set; }
        public string RelatedOrderNote { get; set; }
        public DateTime? ColectionDate { get; set; }
        public string CollectionNote { get; set; }
        public string Notes { get; set; }

        public virtual IList<FileUpload> Files { get; set; }
        public virtual IList<OrderLine> OrderLines { get; set; }
        public virtual IList<OrderCustomerContact> OrderCustomerContacts { get; set; }      

        public Customer Customer { get; set; }
        public ContactType ContactType { get; set; }
    }
}
