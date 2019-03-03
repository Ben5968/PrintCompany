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

        public int OrderId { get; set; }
        public int ItemColorId { get; set; }
        public int ItemSizeId { get; set; }
        public int ItemTypeId { get; set; }
    }
}
