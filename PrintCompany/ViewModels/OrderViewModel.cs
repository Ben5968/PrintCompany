﻿using PrintCompany.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintCompany.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool PrintRequired { get; set; }
        public bool EmbroideryRequired { get; set; }

        public Customer Customer { get; set; }
    }
}
