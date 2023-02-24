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

        public async Task<ResponseBase> AdminRegister(AccountDTO AccountDTO)
        {
            var Admin = mapper.Map<AccountDTO, ApplicationUser>(AccountDTO);
            var Result = await this.authenticationRepository!.CreateAdmin(Admin, AccountDTO.Password!);
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
            try
            {
                var Token = await this.authenticationRepository!.Login(LoginDTO);
                var User = await this.authenticationRepository.GetUserByEmail(LoginDTO.Email!);

                if (User == null) return new TokenResponse
                {
                    IsSuccess = false,
                    Message = "Non user existed",
                    Token = null
                };

                if (!User.EmailConfirmed)
                {
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Account did not confirmed",
                        Token = null
                    };
                }

                string id = await this.tokenService.StoreRefreshToken();
                User.RefreshTokenId = id;
                authenticationRepository.Update(User);
                await unitOfWork.CommitTransaction();

                if (Token == null) return new TokenResponse
            {
                IsSuccess = false,
                Message = "Email or Password got wrong",
                Token = null
            };
            return new TokenResponse
            {
                IsSuccess = true,
                Message = "Welcome back",
                    Token = Token
            };

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw e;
        }
        }

        public async Task<ResponseBase> VendorRegister(AccountDTO AccountDTO)
        {
            var Vendor = mapper.Map<AccountDTO, ApplicationUser>(AccountDTO);
            var Result = await this.authenticationRepository.RegisterVendor(Vendor, AccountDTO.Password!);
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

        public async Task<ResponseBase> RefreshToken(string Email, TokenDTO TokenDTO)
        {
            var Token = await this.authenticationRepository.RefreshToken(Email, TokenDTO);
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
