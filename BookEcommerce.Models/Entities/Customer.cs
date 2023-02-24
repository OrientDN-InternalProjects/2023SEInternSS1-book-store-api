﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.Entities
{
    public class Customer
    {
        public Customer()
        {
            Addresses = new HashSet<Address>();
            Orders = new HashSet<Order>();
            PhoneNumbers = new HashSet<PhoneNumber>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? AccountId { get; set; }
        public virtual ApplicationUser? Account { get; set; }

        public virtual ICollection<Address>? Addresses { get; set; }

        public string? ImageId { get; set; }
        public virtual Image? Image { get; set; }

        public string? CartId { get; set; }
        public virtual List<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<PhoneNumber>? PhoneNumbers { get; set; }

    }
}
