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
        private readonly IMapper Mapper;
        private readonly ILogger logger;
        public TokenService(ITokenRepository tokenRepository, IMapper Mapper, ILogger logger)
        {
            this.tokenRepository = tokenRepository;
            this.Mapper = Mapper;
            this.logger = logger;
        }
        public string CreateRefreshToken()
        {
            var token = this.tokenRepository.CreateRefreshToken();
            return token;
        }

        //public string RefreshToken(string AccessToken)
        //{
        //    return AccessToken;
        //}

        public async Task<string> StoreRefreshToken()
        {
            try
            {
                string Token = this.CreateRefreshToken();
                RefreshTokenStoreViewModel StoreRefreshTokenDTO = new RefreshTokenStoreViewModel
                {
                    Token = Token
                };
                var MapRefreshToken = Mapper.Map<RefreshTokenStoreViewModel, RefreshToken>(StoreRefreshTokenDTO);
                await this.tokenRepository.StoreRefreshToken(MapRefreshToken);
                return MapRefreshToken.RefreshTokenId.ToString()!;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                return null!;
            }
        }
    }
}
