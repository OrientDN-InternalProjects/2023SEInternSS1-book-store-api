﻿using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class ProductViewModel : ResponseBase
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public List<ImageViewModel>? Images { get; set; }
        public List<ProductVariantViewModel>? ProductVariants { get; set; }
    }
}
