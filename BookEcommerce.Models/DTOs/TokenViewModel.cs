﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs
{
    public class TokenViewModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
