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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressRepository;
        private readonly ICustomerService customerService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public AddressService(IAddressRepository addressRepository, ICustomerService customerService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.addressRepository = addressRepository;
            this.customerService = customerService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResponseBase> CreateAddress(AddressViewModel AddressViewModel, string Token)
        {
            try
            {
                var AddressMapper = this.mapper.Map<AddressViewModel, Address>(AddressViewModel);
                Guid? CustomerId = await this.customerService.GetCustomerIdFromToken(Token);
                AddressMapper.CustomerId = CustomerId;
                await this.addressRepository.AddAsync(AddressMapper);
                await unitOfWork.CommitTransaction();
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "Create address successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
