﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrintCompany.Core
{
    public class OrderLine
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public bool PrintRequired { get; set; }
        public bool EmbroideryRequired { get; set; }
        public int PrintQuantity { get; set; }
        public int EmbroideryQuantity { get; set; }
        public int? PrintCompletedQuantity { get; set; }
        public int? EmbroideryCompletedQuantity { get; set; }
        public string Notes { get; set; }
        public string Price { get; set; }

        public int OrderId { get; set; }
        public int ItemColorId { get; set; }
        public int ItemSizeId { get; set; }
        public int ItemTypeId { get; set; }
        public int? SupplierId { get; set; }

        public Order Order { get; set; }
        public ItemColor ItemColor { get; set; }
        public ItemSize ItemSize { get; set; }
        public ItemType ItemType { get; set; }
        public Supplier Supplier { get; set; }

    }
}
