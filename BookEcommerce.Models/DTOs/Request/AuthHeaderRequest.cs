﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Request
{
    public class AuthHeaderRequest
    {
        public string AuthType { get => "Bearer"; }
        public string? Token { get; set; }
    }
}
