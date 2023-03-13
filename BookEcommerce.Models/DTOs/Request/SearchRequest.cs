using BookEcommerce.Models.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DTOs.Request
{
    public class SearchRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
