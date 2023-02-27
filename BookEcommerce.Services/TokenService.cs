using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper Mapper;
        public TokenService(ITokenRepository tokenRepository, IMapper Mapper)
        {
            this.tokenRepository = tokenRepository;
            this.Mapper = Mapper;
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
            string Token = this.CreateRefreshToken();
            RefreshTokenStoreViewModel StoreRefreshTokenDTO = new RefreshTokenStoreViewModel
            {
                Token = Token
            };
            var MapRefreshToken = Mapper.Map<RefreshTokenStoreViewModel, RefreshToken>(StoreRefreshTokenDTO);
            await this.tokenRepository.StoreRefreshToken(MapRefreshToken);
            return MapRefreshToken.RefreshTokenId.ToString()!;
        }
    }
}
