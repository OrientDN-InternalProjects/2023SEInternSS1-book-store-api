﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? ImageId { get; set; }
        public string? ImageURL { get; set; }
        public string? ShopId { get; set; }
        public string? CategoryId { get; set; }
        public string? ItemId { get; set; }
        public string? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Vendor? Vendor { get; set; }
        public virtual Product? Product { get; set; }
    }
}