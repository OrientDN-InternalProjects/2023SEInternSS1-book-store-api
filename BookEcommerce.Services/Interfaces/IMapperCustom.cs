using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface IMapperCustom
    {
        List<ProductViewModel> MapProducts(List<Product> products);
        List<ProductVariantViewModel> MapProductVariant(List<ProductVariant> productsVariant);
    }
}
