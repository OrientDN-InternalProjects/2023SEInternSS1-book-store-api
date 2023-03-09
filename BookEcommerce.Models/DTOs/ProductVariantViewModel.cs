﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class ProductVariantViewModel
    {
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public string? ProductVariantName { get; set; }
        public double ProductNowPrice { get; set; }
        public double ProductDefaultPrice { get; set; }
        public double ProductSalePrice { get; set; }
    }
}
