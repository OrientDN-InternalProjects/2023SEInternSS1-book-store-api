using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
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
        private readonly ICartRepository cartRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork, ITokenRepository tokenRepository, ICartRepository cartRepository)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.tokenRepository = tokenRepository;
            this.cartRepository = cartRepository;
        }

        public async Task<ResponseBase> CreateCustomer(CustomerViewModel customerViewModel, string token)
        {
            Guid userId = tokenRepository.GetUserIdFromToken(token);

            //var MapCustomer = Mapper.Map<customerViewModel, Customer>();
            var customer = new Customer
            {
                FullName = customerViewModel.FullName,
                AccountId = userId.ToString()
            };
            await this.customerRepository.AddAsync(customer);
            var cart = new Cart
            {
                CustomerId = customer.CustomerId
            };
            await this.cartRepository.AddAsync(cart);
            await unitOfWork.CommitTransaction();

            return new ResponseBase
            {
                IsSuccess = true,
                Message = "A Customer's created"
            };
        }

        public async Task<Guid> GetCustomerIdFromToken(string token)
        {
            var userId = this.tokenRepository.GetUserIdFromToken(token);
            var customer = await this.customerRepository.FindAsync(v => v.AccountId!.Equals(userId.ToString()));
            return customer.CustomerId;
        }

        public async Task<string> GetCustomerEmailFromToken(string token)
        {
            var userId = this.tokenRepository.GetUserIdFromToken(token);
            var customer = await this.customerRepository.FindAsync(v => v.AccountId!.Equals(userId.ToString()));
            return customer.Account!.Email;
        }

        public async Task<CustomerResponse> GetCustomerProfile(string token)
        {
            try
            {
                var userId = this.tokenRepository.GetUserIdFromToken(token);
                var customer = await this.customerRepository.FindAsync(v => v.AccountId!.Equals(userId.ToString()));
                return new CustomerResponse
                {
                    IsSuccess = true,
                    Message = "fetched customer profile",
                    CustomerFullName = customer.FullName,
                    CustomerId = customer.CustomerId.ToString()
                };
            }
             catch(Exception e)
            {
                return new CustomerResponse
                {
                    IsSuccess = false,
                    Message = "failed to fetch profile"
                };
            }
        }
    }
}
