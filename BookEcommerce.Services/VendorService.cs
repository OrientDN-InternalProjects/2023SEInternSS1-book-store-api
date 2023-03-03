using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository vendorRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger logger;
        public VendorService(IVendorRepository vendorRepository, IUnitOfWork unitOfWork, ITokenRepository tokenRepository, IMapper mapper, ILogger logger)
        {
            this.vendorRepository = vendorRepository;
            this.unitOfWork = unitOfWork;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ResponseBase> CreateVendor(VendorCreateViewModel createVendorViewModel, string token)
        {
            try
            {
                var userId = tokenRepository.GetUserIdFromToken(token);
                var vendorMapper = this.mapper.Map<VendorCreateViewModel, Vendor>(createVendorViewModel);
                vendorMapper.AccountId = userId.ToString();
                await this.vendorRepository.AddAsync(vendorMapper);
                await this.unitOfWork.CommitTransaction();
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "a vendor's created"
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}. Detail {ex.StackTrace}");
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "create vendor failed"
                };
            }
        }
    }
}
