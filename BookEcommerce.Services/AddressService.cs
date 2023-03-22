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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressRepository;
        private readonly ICustomerService customerService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        public AddressService(IAddressRepository addressRepository, ICustomerService customerService, IMapper mapper, IUnitOfWork unitOfWork, ILogger logger)
        {
            this.addressRepository = addressRepository;
            this.customerService = customerService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<ResponseBase> CreateAddress(AddressViewModel addressViewModel, string token)
        {
            try
            {
                var addressMapper = this.mapper.Map<AddressViewModel, Address>(addressViewModel);
                var customerId = await this.customerService.GetCustomerIdFromToken(token);
                addressMapper.CustomerId = customerId;
                await this.addressRepository.AddAsync(addressMapper);
                await unitOfWork.CommitTransaction();
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "Create address successfully"
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}. Detail {ex.StackTrace}");
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<AddressViewModel>> GetAddressByCus(string token)
        {
            var customerId = await this.customerService.GetCustomerIdFromToken(token);
            var listAddresses = await addressRepository.GetAddressByCusId(customerId);
            var listResult = new List<AddressViewModel>();
            foreach (var address in listAddresses)
            {
                var addressViewModel = new AddressViewModel
                {
                    Country = address.Country,
                    StreetAddress = address.StreetAddress
                };
                listResult.Add(addressViewModel);
            }
            return listResult;
        }
    }
}
