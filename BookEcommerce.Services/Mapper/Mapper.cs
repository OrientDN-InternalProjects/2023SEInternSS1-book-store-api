using AutoMapper;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Mapper
{
    public class Mapper : IMapperCustom
    {
        private readonly IMapper autoMapper;
        public Mapper(IMapper autoMapper)
        {
            this.autoMapper = autoMapper;
        }
        public List<ProductViewModel> MapProducts(List<Product> products)
        {
            var storeProducts = new List<ProductViewModel>();
            foreach (var product in products)
            {
                var item = new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    //Price = MapProductVariant(product.ProductVariants.ToList()),
                };
            }
            throw new NotImplementedException();
        }

        public List<ProductVariantViewModel> MapProductVariant(List<ProductVariant> productsVariant)
        {
            return autoMapper.Map<List<ProductVariant>, List<ProductVariantViewModel>>(productsVariant);
        }
    }
}
