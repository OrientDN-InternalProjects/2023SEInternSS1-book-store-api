using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface ICartService
    {
        public Task<CartResponse> AddCart(CartRequest req,Guid cusId);
        public Task<List<CartViewModel>> GetCart(Guid cusId);
    }
}
