using AutoMapper;
using BookEcommerce.Models.DAL;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DAL.Repositories;
using BookEcommerce.Models.DTOs;
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
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IAuthenticationRepository authenticationRepository,
            IMapper mapper,
            ITokenService tokenService
            )
        {
            this.unitOfWork = unitOfWork;
            this.authenticationRepository = authenticationRepository;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        public async Task<ResponseBase> AdminRegister(AccountViewModel accountViewModel)
        {
            var Admin = mapper.Map<AccountViewModel, ApplicationUser>(accountViewModel);
            var Result = await this.authenticationRepository!.CreateAdmin(Admin, accountViewModel.Password!);
            if (!Result.Succeeded)
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

        public async Task<ResponseBase> CustomerRegister(AccountViewModel accountViewModel)
        {
            var User = mapper.Map<AccountViewModel, ApplicationUser>(accountViewModel);
            //ApplicationUser User = new ApplicationUser
            //{
            //    UserName = accountViewModel.UserName,
            //    Email = accountViewModel.Email,
            //};

            var result = await this.authenticationRepository!.RegisterCustomer(User, accountViewModel.Password!);
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
        public async Task<TokenResponse> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var Token = await this.authenticationRepository!.Login(loginViewModel);
                var User = await this.authenticationRepository.GetUserByEmail(loginViewModel.Email!);

                if (User == null) return new TokenResponse
                {
                    IsSuccess = false,
                    Message = "Non user existed",
                    AccessToken = null,
                    RefreshToken = null
                };

                if (!User.EmailConfirmed)
                {
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Account did not confirmed",
                        AccessToken = null,
                        RefreshToken = null
                    };
                }
                string id = await this.tokenService.StoreRefreshToken();
                User.RefreshTokenId = Guid.Parse(id);
                authenticationRepository.Update(User);
                await unitOfWork.CommitTransaction();
            if (Token == null) 
                return new TokenResponse
                {
                    IsSuccess = false,
                    Message = "Email or Password got wrong",
                    AccessToken = null,
                    RefreshToken = null
                };
            return new TokenResponse
            {
                IsSuccess = true,
                Message = "Welcome back",
                AccessToken = Token.AccessToken,
                RefreshToken = Token.RefreshToken
            };

            }
            catch(Exception e)
            {
                return new TokenResponse
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<ResponseBase> VendorRegister(AccountViewModel accountViewModel)
        {
            var Vendor = mapper.Map<AccountViewModel, ApplicationUser>(accountViewModel);
            var Result = await this.authenticationRepository.RegisterVendor(Vendor, accountViewModel.Password!);
            if (!Result.Succeeded)
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

        public async Task<ResponseBase> RefreshToken(string Email, TokenViewModel tokenViewModel)
        {
            var EmailToUpdateToken = await this.authenticationRepository.GetUserByEmail(Email);
            if (EmailToUpdateToken == null)
            {
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Non user exist"
                };
            }
            var Token = await this.authenticationRepository.RefreshToken(EmailToUpdateToken.Email, tokenViewModel);

            if (Token == null)
            {
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Failed to refresh"
                };
            }
            return new ResponseBase
            {
                IsSuccess = true,
                Message = "Refreshed"
            };
        }
    }
}
