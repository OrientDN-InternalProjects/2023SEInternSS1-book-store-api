using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper Mapper;
        private readonly IUnitOfWork unitOfWork;
        public CustomerService(ICustomerRepository customerRepository, IMapper Mapper, IUnitOfWork unitOfWork)
        {
            this.customerRepository = customerRepository;
            this.Mapper = Mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase> CreateCustomer(CustomerDTO CustomerDTO)
        {
            var MapCustomer = Mapper.Map<CustomerDTO, Customer>(CustomerDTO);
            await this.customerRepository.AddAsync(MapCustomer);
            await unitOfWork.CommitTransaction();

            return new ResponseBase
            {
                IsSuccess = true,
                Message = "A Customer's created"
            };
        }
    }
}
