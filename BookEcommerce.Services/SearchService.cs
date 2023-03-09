using BookEcommerce.Models.DAL;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class SearchService : BaseService, ISearchService
    {
        private readonly IProductRepository productRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly IProductPriceRepository productPriceRepository;
        private readonly IImageRepository imageRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductCategoryRepository productCategoryRepository;
        public SearchService(IUnitOfWork unitOfWork, IProductRepository productRepository, IProductVariantRepository productVariantRepository, 
                             IProductPriceRepository productPriceRepository, IImageRepository imageRepository, ICategoryRepository categoryRepository, IProductCategoryRepository productCategoryRepository) : base(unitOfWork)
        {
            this.productRepository = productRepository;
            this.productVariantRepository = productVariantRepository;
            this.productPriceRepository = productPriceRepository;
            this.imageRepository = imageRepository;
            this.categoryRepository = categoryRepository;
            this.productCategoryRepository = productCategoryRepository;
        }

        private async Task<List<ProductViewModel>> getProduct(List<Product> products)
        {
            var listProducts = new List<ProductViewModel>();
            foreach (var product in products)
            {
                var imageProduct = new List<ImageViewModel>();
                var findImageProduct = await imageRepository.GetImagesByProductId(product.ProductId);
                foreach (var img in findImageProduct)
                {
                    var imgProduct = new ImageViewModel
                    {
                        ImageId = img.ImageId.ToString(),
                        Path = img.ImageURL,
                    };
                    imageProduct.Add(imgProduct);
                }
                var listProductVariants = new List<ProductVariantViewModel>();
                var listProductVariant = await productVariantRepository.GetProductVariantsByIdProduct(product.ProductId);
                foreach (var pv in listProductVariant)
                {
                    var productPrice = await productPriceRepository.GetProductPriceByProductVariantId(pv.ProductVariantId);
                    var productVariantViewModel = new ProductVariantViewModel
                    {
                        ProductVariantId = pv.ProductVariantId,
                        ProductVariantName = pv.ProductVariantName,
                        Quantity = pv.Quantity,
                        ProductDefaultPrice = productPrice.ProductVariantDefaultPrice,
                        ProductSalePrice = productPrice.PruductVariantSalePrice,
                    };
                    listProductVariants.Add(productVariantViewModel);
                }
                var productViewModel = new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDecription,
                    ProductVariants = listProductVariants,
                    Images = imageProduct
                };
                listProducts.Add(productViewModel);
            }
            return listProducts;
        }

        public async Task<List<ProductViewModel>> SearchProductByNameAndType(SearchRequest searchRequest)
        {
            if (string.IsNullOrEmpty(searchRequest.Type))
            {
                return null!;
            }
            else if(searchRequest.Type.Equals("Product"))
            {
                var searchProducts = await productRepository.SearchProduct(searchRequest.Name);
                //if(searchProducts.Count == 0)
                //{
                //    var listProducts = new List<Product>();
                //    var searchDetails = searchRequest.Name.Split(" ");
                //    for(int i = 0; i < searchDetails.Length; i++)
                //    {
                //        var findProductDetails = await productRepository.SearchProduct(searchDetails[i]);
                //        if(findProductDetails.Count != 0)
                //        {
                //            listProducts = findProductDetails;
                //            break;
                //        }
                //    }
                //    var search = await getProduct(listProducts);
                //    return search;
                //}
                var result = await getProduct(searchProducts);
                return result;
            }
            else if(searchRequest.Type.Equals("Category"))
            {
                var searchCategories = await categoryRepository.SearchCategory(searchRequest.Name);
                foreach (var item in searchCategories)
                {
                    var categories = await productCategoryRepository.GetProductCategories(item.CategoryId);
                    var products = new List<Product>();
                    foreach (var category in categories)
                    {
                        var findProduct = await productRepository.GetProductById(category.ProductId);
                        products.Add(findProduct);
                    }
                    return await getProduct(products);
                }
            }
            return null!;
        }

        public async Task<List<Product>> SearchProduct(string name)
        {
            //return await applicationDbContext.Products.Where(p => EF.Functions.Like(p.ProductName!, $"%{name}%")).ToListAsync();
            return await productRepository.SearchProductWithLike(name);
        }
    }
}
