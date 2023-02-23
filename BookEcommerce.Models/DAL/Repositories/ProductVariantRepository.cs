using BookEcommerce.Models.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class ProductVariantRepository : Repository<ProductVariant> , IProductVariantRepository
    {
        public ProductVariantRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
