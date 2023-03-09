using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Base;
using BookEcommerce.Services.Interfaces;

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
        private readonly IProductService productService;
        private readonly IMapperCustom mapper;

        public SearchService(IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IProductVariantRepository productVariantRepository,
            IProductPriceRepository productPriceRepository,
            IImageRepository imageRepository,
            ICategoryRepository categoryRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductService productService,
            IMapperCustom mapper) : base(unitOfWork)
        {
            this.productRepository = productRepository;
            this.productVariantRepository = productVariantRepository;
            this.productPriceRepository = productPriceRepository;
            this.imageRepository = imageRepository;
            this.categoryRepository = categoryRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.mapper = mapper;
            this.productService = productService;
        }

        public async Task<List<ProductViewModel>> SearchProductByNameAndType(SearchRequest searchRequest)
        {
            if ("Product".Equals(searchRequest.Type, StringComparison.InvariantCultureIgnoreCase))
            {
                var searchProducts = await productRepository.SearchProductWithFuzzy(searchRequest.Name);
                var result = mapper.MapProducts(searchProducts);
                return result;
            }
            if ("Category".Equals(searchRequest.Type, StringComparison.InvariantCultureIgnoreCase))
            {
                var searchCategories = await categoryRepository.SearchCategory(searchRequest.Name);
                foreach (var item in searchCategories)
                {
                    var categories = await productCategoryRepository.GetProductCategories(item.CategoryId);
                    var generalProduct = await searchProduct(categories);
                    return mapper.MapProducts(generalProduct);
                }
            }
            return await productService.GetAllProduct();
        }
        
        private async Task<List<Product>> searchProduct(List<ProductCategory> productCategories)
        {
            var products = new List<Product>();
            foreach (var category in productCategories)
            {
                var findProduct = await productRepository.GetProductById(category.ProductId);
                products.Add(findProduct);
            }
            return products;
        }
    }
}
