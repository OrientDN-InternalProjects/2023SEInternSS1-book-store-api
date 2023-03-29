using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DAL.Repositories;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookEcommerce.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly IProductPriceRepository productPriceRepository;
        private readonly IImageRepository imageRepository;
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly IVendorRepository vendorRepository;
        private readonly ILogger<ProductService> logger;
        private readonly IMapperCustom mapper;
        public ProductService
            (IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IProductPriceRepository productPriceRepository,
            IProductVariantRepository productVariantRepository,
            IImageRepository imageRepository,
            IProductCategoryRepository productCategoryRepository,
            ITokenRepository tokenRepository,
            IVendorRepository vendorRepository,
            ILogger<ProductService> logger,
            IMapperCustom mapper
            ) : base(unitOfWork)
        {
            this.productRepository = productRepository;
            this.productVariantRepository = productVariantRepository;
            this.productPriceRepository = productPriceRepository;
            this.imageRepository = imageRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.tokenRepository = tokenRepository;
            this.vendorRepository = vendorRepository;
            this.logger = logger;
            this.mapper = mapper;
        }
        public async Task<ProductResponse> AddProduct(ProductRequest req, string token)
        {
            try
            {
                var userId = this.tokenRepository.GetUserIdFromToken(token);
                var vendor = await this.vendorRepository.FindAsync(v => v.AccountId!.Equals(userId.ToString()));
                var product = new Product
                {
                    ProductName = req.ProductName,
                    ProductDecription = req.ProductDescription,
                    VendorId = vendor.VendorId,
                    IsActive = true,
                    DateCreated = DateTime.Now,
                };
                await productRepository.AddAsync(product);
                await addImageAndProductCategory(req, product);
                await addProductVariant(req.ProductVariants!, product);
                await _unitOfWork.CommitTransaction();
                return new ProductResponse
                {
                    IsSuccess = true,
                    Message = "Add Product Success and you can view Product",
                    Link = "https://localhost:7018/api/product/" + product.ProductId.ToString()
                };
            }
            catch (NullReferenceException e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new ProductResponse
                {
                    IsSuccess = false,
                    Message = "Can't find Product! "
                };
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new ProductResponse
                {
                    IsSuccess = false,
                    Message = "System error, Add Product was fasle and please try again later! "
                };
            }
        }

        public async Task<List<ProductViewModel>> GetAllProduct()
        {
            var listProducts = await productRepository.GetAll();
            return mapper.MapProducts(listProducts);
        }

        public async Task<ProductViewModel> GetProductById(Guid productId)
        {
            try
            {
                var findProduct = await productRepository.GetProductById(productId);
                var findProductVariant = await productVariantRepository.GetProductVariantsByIdProduct(findProduct.ProductId);
                var findImageProduct = await imageRepository.GetImagesByProductId(findProduct.ProductId);
                return new ProductViewModel
                {
                    IsSuccess = true,
                    ProductId = findProduct.ProductId,
                    ProductName = findProduct.ProductName,
                    VendorId = findProduct.Vendor!.VendorId,
                    VendorName = findProduct.Vendor.FullName,
                    ProductDescription = findProduct.ProductDecription,
                    Sold = findProduct.Sold,
                    Created = findProduct.DateCreated,
                    ProductVariants = mapper.MapProductVariant(findProductVariant),
                    Images = mapper.MapImages(findImageProduct),
                };
            }
            catch (NullReferenceException e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new ProductViewModel
                {
                    IsSuccess = false,
                    Message = "Some properties is Null! "
                };
            }
            catch (InvalidOperationException e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new ProductViewModel
                {
                    IsSuccess = false,
                    Message = "Can't find Product in our System! "
                };
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new ProductViewModel 
                { 
                    IsSuccess = false, 
                    Message = "System error, Get Product was fasle and please try again later! "
                };
            }
        }
        private async Task<ProductResponse> addProductVariant(List<ProductVariantRequest> listProductsVariant, Product product)
        {
            foreach (var proVariant in listProductsVariant)
            {
                if (proVariant.Quantity == 0 || proVariant.ProductDefautPrice == 0 || proVariant.ProductSalePrice == 0)
                {
                    logger.LogError("Vendor enter value 0 when adding product");
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
                        ProductVariantSalePrice = proVariant.ProductSalePrice,
                        ActivationDate = proVariant.ActivationDate,
                        ExpirationDate = proVariant.ExpirationDate,
                        ProductVariantId = productVariant.ProductVariantId,
                    };
                    await productPriceRepository.AddAsync(productPrice);
                }
            }
            return new ProductResponse
            {
                IsSuccess = true
            };
        }
        private async Task addImageAndProductCategory(ProductRequest req, Product product)
        {
            foreach (var path in req.Paths!)
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
        }

        public async Task<List<ProductViewModel>> GetProductMostSellProduct()
        {
            var listProduct = await productRepository.GetProductsMostSeller();
            return mapper.MapProducts(listProduct);
        }

        public async Task<List<ProductViewModel>> GetProductTopNew()
        {
            var listProducts = await productRepository.GetProductsTopNew();
            return mapper.MapProducts(listProducts);
        }
    }
}
