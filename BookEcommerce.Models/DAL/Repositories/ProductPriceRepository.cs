using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class ProductPriceRepository : Repository<ProductPrice>, IProductPriceRepository
    {
        public ProductPriceRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
