using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface ISearchService
    {
        public Task<List<Product>> SearchProduct(string name);
        Task<List<ProductViewModel>> SearchProductByNameAndType(SearchRequest searchRequest);
    }
}
