﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookEcommerce.Models.Entities
{
    public class BankProvider
    {
        public BankProvider()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? BankProviderId { get; set; }
        public string? BankProviderName { get; set; }
        public string? BankProviderShortName { get; set; }

    }
}
