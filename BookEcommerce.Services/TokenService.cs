using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookEcommerce.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;
        private readonly ILogger logger;
        public TokenService(ITokenRepository tokenRepository, IMapper mapper, ILogger logger)
        {
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public string CreateRefreshToken()
        {
            var token = this.tokenRepository.CreateRefreshToken();
            return token;
        }

        public string GetUserEmailFromToken(string token)
        {
            var email = this.tokenRepository.GetUserEmailFromToken(token);
            return email;
        }

        public string GetUserIdFromToken(string token)
        {
            var id = this.tokenRepository.GetUserIdFromToken(token);
            return id.ToString();
        }

        public async Task<string> StoreRefreshToken()
        {
            try
            {
                string token = this.CreateRefreshToken();
                var storeRefreshTokenViewModel = new RefreshTokenStoreViewModel
                {
                    Token = token
                };
                var mapRefreshToken = mapper.Map<RefreshTokenStoreViewModel, RefreshToken>(storeRefreshTokenViewModel);
                await this.tokenRepository.StoreRefreshToken(mapRefreshToken);
                return mapRefreshToken.RefreshTokenId.ToString()!;
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}. Detail {ex.StackTrace}");
                return null!;
            }
        }
    }
}
