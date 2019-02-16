﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrintCompany.Core
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int ItemCount { get; set; }

        public int OrderId { get; set; }
        public int ItemColorId { get; set; }
        public int ItemSizeId { get; set; }
        public int ItemTypeId { get; set; }
                     
        public Order Order { get; set; }
        public ItemColor ItemColor { get; set; }
        public ItemSize ItemSize { get; set; }
        public ItemType ItemType { get; set; }


    }
}
