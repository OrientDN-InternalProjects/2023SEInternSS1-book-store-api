using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IProductPriceRepository : IRepository<ProductPrice>
    {
        Task<ProductPrice> GetProductPriceByProductVariantId(Guid productVariantId);
        Task<double> GetPriceByProductVariant(Guid productVariantId);
    }
}
