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
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper Mapper;
        private readonly IUnitOfWork unitOfWork;
        public CustomerService(ICustomerRepository customerRepository, IMapper Mapper, IUnitOfWork unitOfWork, ITokenRepository tokenRepository)
        {
            this.customerRepository = customerRepository;
            this.Mapper = Mapper;
            this.unitOfWork = unitOfWork;
            this.tokenRepository = tokenRepository;
        }

        public async Task<ResponseBase> CreateCustomer(CustomerDTO CustomerDTO, string Token)
        {
            string UserId = tokenRepository.GetUserIdFromToken(Token);

            //var MapCustomer = Mapper.Map<CustomerDTO, Customer>();
            Customer customer = new Customer
            {
                FullName = CustomerDTO.FullName,
                AccountId = UserId
            };
            await this.customerRepository.AddAsync(customer);
            await unitOfWork.CommitTransaction();

            return new ResponseBase
            {
                IsSuccess = true,
                Message = "A Customer's created"
            };
        }
    }
}
