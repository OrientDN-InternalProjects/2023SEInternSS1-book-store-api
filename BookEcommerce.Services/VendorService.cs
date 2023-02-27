using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
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
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository vendorRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public VendorService(IVendorRepository vendorRepository, IUnitOfWork unitOfWork, ITokenRepository tokenRepository, IMapper mapper)
        {
            this.vendorRepository = vendorRepository;
            this.unitOfWork = unitOfWork;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
        }

        public async Task<ResponseBase> CreateVendor(VendorCreateViewModel CreateVendorDTO, string Token)
        {
            try
            {
                string UserId = tokenRepository.GetUserIdFromToken(Token);
                var VendorMapper = this.mapper.Map<VendorCreateViewModel, Vendor>(CreateVendorDTO);
                VendorMapper.AccountId = Guid.Parse(UserId);
                await this.vendorRepository.AddAsync(VendorMapper);
                await this.unitOfWork.CommitTransaction();
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "a vendor's created"
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
