using System;
using System.Collections.Generic;
using System.Text;

namespace PrintCompany.Core
{
    public class SQLViewOrder
    {
        public int OrderNo { get; set; }
        //public int CustomerId { get; set; }
        public string CustomerName { get; set; }  
        public DateTime OrderDate { get; set; }        
        public DateTime? DueDate { get; set; }
        public string InvoiceNumber { get; set; }       
        public DateTime? InvoiceDate { get; set; }
        public int QuantityTotalByOrder { get; set; }
        public int PrintQuantityTotalByOrder { get; set; }
        public int EmbroideryQuantityTotalByOrder { get; set; }
        public int PrintQuantityCompletedTotalByOrder { get; set; }
        public int EmbroideryQuantityCompletedTotalByOrder { get; set; }
        public string OrderStatus { get; set; }
    }
}
