﻿using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IProductVariantRepository : IRepository<ProductVariant>
    {
        Task<List<ProductVariant>> GetProductVariantsByIdProduct(Guid productId);
        Task<ProductVariant> GetProductVariantById(Guid productVariantId);
    }
}
