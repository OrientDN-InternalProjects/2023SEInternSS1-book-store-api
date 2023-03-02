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
        private readonly IMapper Mapper;
        private readonly IUnitOfWork unitOfWork;
        public CustomerService(ICustomerRepository customerRepository, IMapper Mapper, IUnitOfWork unitOfWork, ITokenRepository tokenRepository, ICartRepository cartRepository)
        {
            this.customerRepository = customerRepository;
            this.Mapper = Mapper;
            this.unitOfWork = unitOfWork;
            this.tokenRepository = tokenRepository;
            this.cartRepository = cartRepository;
        }

        public async Task<ResponseBase> CreateCustomer(CustomerViewModel CustomerDTO, string Token)
        {
            Guid UserId = tokenRepository.GetUserIdFromToken(Token);

            //var MapCustomer = Mapper.Map<CustomerDTO, Customer>();
            Customer customer = new Customer
            {
                FullName = CustomerDTO.FullName,
                AccountId = UserId.ToString()
            };
            await this.customerRepository.AddAsync(customer);
            Cart cart = new Cart
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

        public async Task<Guid> GetCustomerIdFromToken(string Token)
        {
            var userId = this.tokenRepository.GetUserIdFromToken(Token);
            var customer = await this.customerRepository.FindAsync(v => v.AccountId.Equals(userId.ToString()));
            return customer.CustomerId;
        }

        public async Task<string> GetCustomerEmailFromToken(string Token)
        {
            var userId = this.tokenRepository.GetUserIdFromToken(Token);
            var customer = await this.customerRepository.FindAsync(v => v.AccountId.Equals(userId.ToString()));
            return customer.Account!.Email;
        }
    }
}
