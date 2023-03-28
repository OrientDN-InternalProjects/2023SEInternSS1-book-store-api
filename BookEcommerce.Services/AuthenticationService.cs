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
using Microsoft.Extensions.Logging;
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
        private readonly ITokenRepository tokenRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IAuthenticationRepository authenticationRepository,
            IMapper mapper,
            ITokenService tokenService,
            ILogger logger,
            ITokenRepository tokenRepository
        )
        {
            this.unitOfWork = unitOfWork;
            this.authenticationRepository = authenticationRepository;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.logger = logger;
            this.tokenRepository = tokenRepository;
        }

        public async Task<ResponseBase> AdminRegister(AccountViewModel accountViewModel)
        {
            try
            {
                var admin = mapper.Map<AccountViewModel, ApplicationUser>(accountViewModel);
                var result = await this.authenticationRepository!.CreateAdmin(admin, accountViewModel.Password!);
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
                    Message = "Create account successfully"
                };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Cannot create account, please try again"
                };
            }
        }

        public async Task<ResponseBase> CustomerRegister(AccountViewModel accountViewModel)
        {
            try
            {
                var user = mapper.Map<AccountViewModel, ApplicationUser>(accountViewModel);
                var result = await this.authenticationRepository!.RegisterCustomer(user, accountViewModel.Password!);
                await this.unitOfWork!.CommitTransaction();
                if (result.Succeeded)
                {
                    return new ResponseBase
                    {
                        IsSuccess = true,
                        Message = "Create account successfully"
                    };
                }
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Cannot create account, please try again"
                };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Cannot create account, please try again"
                };
            }
        }

        public async Task<TokenResponse> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var token = await this.authenticationRepository!.Login(loginViewModel);
                var user = await this.authenticationRepository.GetUserByEmail(loginViewModel.Email!);

                if (user == null) return new TokenResponse
                {
                    IsSuccess = false,
                    Message = "Non user existed",
                };

                if (!user.EmailConfirmed)
                {
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        IsActive = false,
                        Message = "Account did not confirmed",
                    };
                }
                string id = await this.tokenService.StoreRefreshToken(token.RefreshToken!);
                user.RefreshTokenId = Guid.Parse(id);
                authenticationRepository.Update(user);
                await unitOfWork.CommitTransaction();
                if (token == null) 
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
                    IsActive = true,
                    Message = "Welcome back",
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                return new TokenResponse
                {
                    IsSuccess = false,
                    Message = "Somethings went wrong, please try again"
                };
            }
        }

        public async Task<ResponseBase> VendorRegister(AccountViewModel accountViewModel)
        {
            try
            {
                var vendor = mapper.Map<AccountViewModel, ApplicationUser>(accountViewModel);
                var result = await this.authenticationRepository.RegisterVendor(vendor, accountViewModel.Password!);
                if (!result.Succeeded)
                {
                    return new ResponseBase
                    {
                        IsSuccess = false,
                        Message = "Email or Password got wrong"
                    };
                }
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "Create account successfully"
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                return new TokenResponse
                {
                    IsSuccess = false,
                    Message = "Somethings went wrong, please try again"
                };
            }
        }

        public async Task<TokenResponse> RefreshToken(string email, TokenViewModel tokenViewModel)
        {
            try
            {
                var emailToUpdateToken = await this.authenticationRepository.GetUserByEmail(email);
                if (emailToUpdateToken == null)
                {
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Non user exist"
                    };
                }
                var existRefreshToken = this.tokenRepository.GetRefreshTokenExist(tokenViewModel.RefreshToken!);
                if (existRefreshToken == null)
                {
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Failed to refresh"
                    };
                }
                var token = this.authenticationRepository.RefreshToken(emailToUpdateToken.Email, tokenViewModel);
                var storeToken = this.tokenService.CreateRefreshToken();
                var tokenStored = await this.tokenService.StoreRefreshToken(storeToken);
                emailToUpdateToken.RefreshTokenId = Guid.Parse(tokenStored);
                this.authenticationRepository.Update(emailToUpdateToken);
                await unitOfWork.CommitTransaction();
                if (token == null)
                {
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        Message = "Failed to refresh"
                    };
                }
                return new TokenResponse
                {
                    IsSuccess = true,
                    Message = "Refreshed",
                    AccessToken = token,
                    RefreshToken = storeToken
                };
            }
            catch(Exception ex)
            {
                logger.LogError($"{ex.Message}. Detail {ex.StackTrace}");
                return new TokenResponse
                {
                    IsSuccess = false,
                    Message = "Somethings went wrong, please try again"
                };
            }
        }

        public UserLoggedResponse GetUserLogged(string token)
        {
            var id = this.tokenService.GetUserIdFromToken(token);
            var email = this.tokenService.GetUserEmailFromToken(token);
            return new UserLoggedResponse
            {
                IsSuccess = true,
                Message = "return logged user",
                UserId = id,
                UserName = email
            };
        }
    }
}
