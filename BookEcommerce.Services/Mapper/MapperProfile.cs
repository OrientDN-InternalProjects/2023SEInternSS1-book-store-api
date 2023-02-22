using AutoMapper;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AccountDTO, ApplicationUser>();
            CreateMap<ApplicationUser, AccountDTO>();
            CreateMap<StoreRefreshTokenDTO, RefreshToken>();
            CreateMap<RefreshToken, StoreRefreshTokenDTO>();
            CreateMap<CustomerDTO, Customer>();
        }
    }
}
