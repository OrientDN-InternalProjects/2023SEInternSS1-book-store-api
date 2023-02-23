using AutoMapper;
using BookEcommerce.Models.DAL;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DAL.Repositories;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
using BookEcommerce.Services.Mapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository authenticationRepository;
        //private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthenticationService(
            //IUnitOfWork unitOfWork, 
            IAuthenticationRepository authenticationRepository,
            IMapper mapper
            )
        {
            //this.unitOfWork = unitOfWork;
            this.authenticationRepository = authenticationRepository;
            this.mapper = mapper;
        }

        public async Task<ResponseBase> CustomerRegister(AccountDTO AccountDTO)
        {
            var User = mapper.Map<AccountDTO, ApplicationUser>(AccountDTO);
            //ApplicationUser User = new ApplicationUser
            //{
            //    UserName = AccountDTO.UserName,
            //    Email = AccountDTO.Email,
            //};

            var result = await this.authenticationRepository!.RegisterCustomer(User, AccountDTO.Password!);
            //await this.unitOfWork!.CommitTransaction();
            if (result.Succeeded)
            {
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "Create a new user"
                };
            }
            else
            {
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Somethings went wrong!"
                };
            }
        }
        public async Task<TokenResponse> Login(LoginDTO LoginDTO)
        {
            var token = await this.authenticationRepository!.Login(LoginDTO);
            if (token == null) return new TokenResponse
            {
                IsSuccess = false,
                Message = "Email or Password got wrong",
                Token = null
            };
            return new TokenResponse
            {
                IsSuccess = true,
                Message = "Welcome back",
                Token = token
            };
        }

        public async Task<ResponseBase> VendorRegister(AccountDTO AccountDTO)
        {
            var Vendor = mapper.Map<AccountDTO, ApplicationUser>(AccountDTO);
            var result = await this.authenticationRepository.RegisterVendor(Vendor, AccountDTO.Password!);
            if (!result.Succeeded)
            {
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Somethings went wrong!"
                };
            }
            return new ResponseBase
            {
                IsSuccess = true,
                Message = "Create a new vendor"
            };
        }
    }
}
