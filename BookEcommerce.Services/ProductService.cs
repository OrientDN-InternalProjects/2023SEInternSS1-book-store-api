using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Base;
using BookEcommerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly IProductPriceRepository productPriceRepository;
        private readonly IImageRepository imageRepository;
        private readonly IProductCategoryRepository productCategoryRepository;
        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, IProductPriceRepository productPriceRepository
                              , IProductVariantRepository productVariantRepository, IImageRepository imageRepository, IProductCategoryRepository productCategoryRepository) : base(unitOfWork)
        {
            this.productRepository = productRepository;
            this.productVariantRepository = productVariantRepository;
            this.productPriceRepository = productPriceRepository;
            this.imageRepository = imageRepository;
            this.productCategoryRepository = productCategoryRepository;
        }

        public async Task<ProductResponse> AddProduct(ProductRequest req)
        {
            try
            {
                var product = new Product
                {
                    ProductName = req.ProductName,
                    ProductDecription = req.ProductDescription,
                    VendorId = req.VendorId,
                };
                await productRepository.AddAsync(product);
                foreach (var path in req.Paths)
                {
                    var img = new Image
                    {
                        ImageURL = path,
                        ProductId = product.ProductId
                    };
                    await imageRepository.AddAsync(img);
                }
                var cate = new ProductCategory
                {
                    CategoryId = req.CategoryId,
                    ProductId = product.ProductId
                };
                await productCategoryRepository.AddAsync(cate);
                foreach (var proVariant in req.ProductVariants)
                {
                    if (proVariant.Quantity == 0 || proVariant.ProductDefautPrice == 0 || proVariant.ProductSalePrice == 0)
                    {
                        return new ProductResponse
                        {
                            IsSuccess = false,
                            Message = "Can't enter value 0 when adding product"
                        };
                    }
                    else
                    {
                        var productVariant = new ProductVariant
                        {
                            ProductVariantName = proVariant.ProductVariantName,
                            Quantity = proVariant.Quantity,
                            ProductId = product.ProductId
                        };
                        await productVariantRepository.AddAsync(productVariant);
                        var productPrice = new ProductPrice
                        {
                            ProductVariantDefaultPrice = proVariant.ProductDefautPrice,
                            PruductVariantSalePrice = proVariant.ProductSalePrice,
                            ActivationDate = proVariant.ActivationDate,
                            ExpirationDate = proVariant.ExpirationDate,
                            ProductVariantId = productVariant.ProductVariantId,
                        };
                        await productPriceRepository.AddAsync(productPrice);
                    }
                }
                await _unitOfWork.CommitTransaction();
                return new ProductResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                throw e;
                //return new ProductResponse
                //{
                //    IsSuccess = false,
                //    Message = e.Message
                //};
            }
        }
    }
}
