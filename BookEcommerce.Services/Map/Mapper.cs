﻿using AutoMapper;
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
    public class MapperCustom : IMapperCustom
    {
        private readonly IMapper autoMapper;
        public MapperCustom(IMapper autoMapper)
        {
            this.autoMapper = autoMapper;
        }

        public List<ImageViewModel> MapImages(List<Image> images)
        {
            return autoMapper.Map<List<Image>, List<ImageViewModel>>(images);
        }

        private int quantitiesProduct(List<ProductVariantViewModel>? productVariantViewModels)
        {
            var quantities = 0;
            foreach (var item in productVariantViewModels!)
            {
                quantities += item.Quantity;
            }
            return quantities;
        }

        public List<ProductViewModel> MapProducts(List<Product> products)
        {
            var storeProducts = new List<ProductViewModel>();
            foreach (var item in products)
            {
                var product = new ProductViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductDescription = item.ProductDecription,
                    VendorId = item.VendorId!.Value,
                    VendorName = item.Vendor!.FullName,
                    Sold = item.Sold,
                    Created = item.DateCreated,
                    Quantity = quantitiesProduct(MapProductVariant((item.ProductVariants!).ToList())),
                    ProductVariants = MapProductVariant((item.ProductVariants!).ToList()),
                    Images = MapImages((item.Images!).ToList())
                };
                storeProducts.Add(product);
            }
            return storeProducts;
        }

        public List<ProductVariantViewModel> MapProductVariant(List<ProductVariant> productsVariants)
        {
            var storeProductVariants = new List<ProductVariantViewModel>();
            foreach (var item in productsVariants)
            {
                
                if(item.ProductPrice!.ExpirationDate < DateTime.Now)
                {
                    var productVariant = new ProductVariantViewModel
                    {
                        ProductVariantId = item.ProductVariantId,
                        VendorId = item.Product!.Vendor!.VendorId,
                        ProductVariantName = item.ProductVariantName,
                        Quantity = item.Quantity,
                        ProductDefaultPrice = item.ProductPrice!.ProductVariantDefaultPrice,
                        ProductSalePrice = item.ProductPrice!.ProductVariantSalePrice,
                        ProductNowPrice = item.ProductPrice!.ProductVariantDefaultPrice
                    };
                    storeProductVariants.Add(productVariant);
                }
                else
                {
                    var productVariant = new ProductVariantViewModel
                    {
                        ProductVariantId = item.ProductVariantId,
                        VendorId = item.Product!.Vendor!.VendorId,
                        ProductVariantName = item.ProductVariantName,
                        Quantity = item.Quantity,
                        ProductDefaultPrice = item.ProductPrice!.ProductVariantDefaultPrice,
                        ProductSalePrice = item.ProductPrice!.ProductVariantSalePrice,
                        ProductNowPrice = item.ProductPrice!.ProductVariantSalePrice
                    };
                    storeProductVariants.Add(productVariant);
                }
            }
            return storeProductVariants;
        }
    }
}
