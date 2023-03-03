using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductService> logger;
        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, IProductPriceRepository productPriceRepository
                              , IProductVariantRepository productVariantRepository, IImageRepository imageRepository, IProductCategoryRepository productCategoryRepository,
                                ILogger<ProductService> logger) : base(unitOfWork)
        {
            this.productRepository = productRepository;
            this.productVariantRepository = productVariantRepository;
            this.productPriceRepository = productPriceRepository;
            this.imageRepository = imageRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.logger = logger;
        }
        public async Task<ProductResponse> AddProductVariant(List<ProductVariantRequest> listProductsVariant, Product product)
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
                        PruductVariantSalePrice = proVariant.ProductSalePrice,
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
        public async Task AddImageAndProductCategory(ProductRequest req , Product product)
        {
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
                    IsActive = true,
                };
                await productRepository.AddAsync(product);
                await AddImageAndProductCategory(req, product);
                if(!AddProductVariant(req.ProductVariants, product).Result.IsSuccess)
                {
                    return await AddProductVariant(req.ProductVariants, product);
                };
                await _unitOfWork.CommitTransaction();
                return new ProductResponse
                {
                    IsSuccess = true,
                    Message = "Add Order Success and you can view Product in Link: https://localhost:7018/api/product/" + product.ProductId.ToString() 
                };
            }
            catch (NullReferenceException)
            {
                logger.LogError("Some properties is Null when Vendor Update Order! ");
                return new ProductResponse
                {
                    IsSuccess = false,
                    Message = "Can't find Product! "
                };
            }
            catch (Exception)
            {
                logger.LogError("System error and Exception was not found when Add Product! ");
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
            var listProductsViewModel = new List<ProductViewModel>();
            foreach (var item in listProducts)
            {
                var listProductVariants = new List<ProductVariantViewModel>();
                var listProductVariant = await productVariantRepository.GetProductVariantsByIdProduct(item.ProductId);
                foreach (var pv in listProductVariant)
                {
                    var productPrice = await productPriceRepository.GetProductPriceByProductVariantId(pv.ProductVariantId);
                    var productVariantViewModel = new ProductVariantViewModel
                    {
                        ProductVariantId = pv.ProductVariantId,
                        ProductVariantName = pv.ProductVariantName,
                        ProductDefaultPrice = productPrice.ProductVariantDefaultPrice,
                        ProductSalePrice = productPrice.PruductVariantSalePrice,
                    };
                    listProductVariants.Add(productVariantViewModel);
                }
                var productViewModel = new ProductViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductDescription = item.ProductDecription,
                    ProductVariants = listProductVariants
                };
                listProductsViewModel.Add(productViewModel);
            }
            return listProductsViewModel;
        }

        public async Task<ProductViewModel> GetProductById(Guid productId)
        {
            try
            {
                var findProduct = await productRepository.GetProductById(productId);
                var findProductVariant = await productVariantRepository.GetProductVariantsByIdProduct(findProduct.ProductId);
                var findImageProduct = await imageRepository.GetImagesByProductId(findProduct.ProductId);
                var imageProduct = new List<ImageViewModel>();
                foreach (var img in findImageProduct)
                {
                    var imgProduct = new ImageViewModel
                    {
                        ImageId = img.ImageId.ToString(),
                        Path = img.ImageURL,
                    };
                    imageProduct.Add(imgProduct);
                }
                var listProductVariant = new List<ProductVariantViewModel>();
                foreach (var item in findProductVariant)
                {
                    var productPrice = await productPriceRepository.GetProductPriceByProductVariantId(item.ProductVariantId);
                    var productVariant = new ProductVariantViewModel
                    {
                        ProductVariantId = item.ProductVariantId,
                        ProductVariantName = item.ProductVariantName,
                        ProductDefaultPrice = productPrice.ProductVariantDefaultPrice,
                        ProductSalePrice = productPrice.PruductVariantSalePrice,
                    };
                    listProductVariant.Add(productVariant);
                }
                return new ProductViewModel
                {
                    IsSuccess = true,
                    ProductId = findProduct.ProductId,
                    ProductName = findProduct.ProductName,
                    ProductDescription = findProduct.ProductDecription,
                    ProductVariants = listProductVariant,
                    Images = imageProduct,
                };
            }
            catch (NullReferenceException)
            {
                logger.LogError("Some properties is Null when Get Product Detail! ");
                return new ProductViewModel
                {
                    IsSuccess = false,
                    Message = "Can't find Product! "
                };
            }
            catch (InvalidOperationException)
            {
                logger.LogError("ProductId is valid when Get Product Detail! ");
                return new ProductViewModel
                {
                    IsSuccess = false,
                    Message = "Can't find Product in our System! "
                };
            }
            catch (Exception)
            {
                logger.LogError("System error and Exception was not found when Get Details Product! ");
                return new ProductViewModel 
                { 
                    IsSuccess = false, 
                    Message = "System error, Add Product was fasle and please try again later! "
                };
            }
        }
    }
}
