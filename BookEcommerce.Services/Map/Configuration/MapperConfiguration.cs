﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Mapper.Configuration
{
    public class MapperConfiguration
    {
        public IMapper Mapper;
        public MapperConfiguration(IMapper Mapper)
        {
            this.Mapper = Mapper;
        }
    }
}
