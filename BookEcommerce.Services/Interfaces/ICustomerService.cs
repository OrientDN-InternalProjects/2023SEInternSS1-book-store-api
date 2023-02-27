using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<ResponseBase> CreateCustomer(CustomerViewModel CustomerDTO, string Token);
    }
}
