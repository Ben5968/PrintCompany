using PrintCompany.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintCompany.ViewModels
{
    public class OrderLineViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool PrintRequired { get; set; }
        public bool EmbroideryRequired { get; set; }
        public int? PrintCompletedQuantity { get; set; }
        public int? EmbroideryCompletedQuantity { get; set; }
        public int PrintQuantity { get; set; }
        public int EmbroideryQuantity { get; set; }
        public string Notes { get; set; }

        public int OrderId { get; set; }
        public int ItemColorId { get; set; }
        public int ItemSizeId { get; set; }
        public int ItemTypeId { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }

        public ItemColor ItemColor { get; set; }
        public ItemSize ItemSize { get; set; }
        public ItemType ItemType { get; set; }
        public Supplier Supplier { get; set; }
    }
}
