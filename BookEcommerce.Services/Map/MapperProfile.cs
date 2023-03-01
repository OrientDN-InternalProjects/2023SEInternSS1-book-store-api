using AutoMapper;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Map
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AccountViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, AccountViewModel>();
            CreateMap<RefreshTokenStoreViewModel, RefreshToken>();
            CreateMap<RefreshToken, RefreshTokenStoreViewModel>();
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<VendorCreateViewModel, Vendor>();
            CreateMap<Vendor, VendorCreateViewModel>();
        }
    }
}
