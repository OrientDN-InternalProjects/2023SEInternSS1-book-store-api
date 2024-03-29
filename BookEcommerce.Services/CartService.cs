﻿using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
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
    public class CartService : BaseService, ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly IProductPriceRepository productPriceRepository;
        private readonly ICartDetailRepository cartDetailRepository;
        public CartService(IUnitOfWork unitOfWork, ICartRepository cartRepository, 
                           IProductVariantRepository productVariantRepository, IProductPriceRepository productPriceRepository,ICartDetailRepository cartDetailRepository) : base(unitOfWork)
        {
            this.productVariantRepository = productVariantRepository;
            this.cartRepository = cartRepository;
            this.productPriceRepository = productPriceRepository;
            this.cartDetailRepository = cartDetailRepository;
        }
        public async Task<CartResponse> AddCart(CartRequest req, Guid cusId)
        {
            try
            {
                var findCart = await cartRepository.GetCartByCustomerId(cusId);
                if (findCart == null)
                {
                    var createCart = new Cart
                    {
                        CustomerId = cusId,
                    };
                    await cartRepository.AddAsync(createCart);   
                    var createCartDetail = new CartDetail
                    {
                        ProductVariantId = req.ProductVariantId,
                        Quantity = req.Quantity,
                    };
                    await cartDetailRepository.AddAsync(createCartDetail);
                }
                else 
                {
                    var findCartDetail = await cartDetailRepository.GetListCartDetailByCartId(findCart.CartId);
                    foreach (var item in findCartDetail)
                    {
                        if (item.ProductVariantId.Equals(req.ProductVariantId))
                        {
                            item.Quantity += req.Quantity;
                            await _unitOfWork.CommitTransaction();
                            return new CartResponse
                            {
                                IsSuccess = true,
                                Message = "Increased the number of this product in the cart Success!"
                            };
                        }
                    }
                    var createCartDetail = new CartDetail
                    {
                        ProductVariantId = req.ProductVariantId,
                        Quantity = req.Quantity,
                        CartId = findCart.CartId,
                    };
                    await cartDetailRepository.AddAsync(createCartDetail);
                }
                await _unitOfWork.CommitTransaction();
                return new CartResponse
                {
                    IsSuccess = true,
                    Message = "Add Product To Cart Success!"
                };
            }
            catch (Exception e)
            {
                return new CartResponse 
                { 
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<List<CartViewModel>> GetCart(Guid cusId)
        {
            try
            {
                var findCart = await cartRepository.GetCartByCustomerId(cusId);
                var cart = new List<CartViewModel>();
                var findListCartDetail = await cartDetailRepository.GetListCartDetailByCartId(findCart.CartId);
                foreach (var item in findListCartDetail)
                {
                    var findProductVariant = await productVariantRepository.GetProductVariantById(item.ProductVariantId);
                    var findProductPrice = await productPriceRepository.GetProductPriceByProductVariantId(findProductVariant.ProductVariantId);
                    if(findProductPrice.ExpirationDate < DateTime.Now)
                    {
                        var createCartViewModel = new CartViewModel
                        {
                            IsSuccess = true,
                            ProductVariantId = item.ProductVariantId,
                            ProductVariantName = findProductVariant.ProductVariantName,
                            Quantity = item.Quantity,
                            Total = findProductPrice.ProductVariantDefaultPrice * item.Quantity
                        };
                        cart.Add(createCartViewModel);
                    }
                    else
                    {
                        var createCartViewModel = new CartViewModel
                        {
                            IsSuccess = true,
                            ProductVariantId = item.ProductVariantId,
                            ProductVariantName = findProductVariant.ProductVariantName,
                            Quantity = item.Quantity,
                            Total = findProductPrice.PruductVariantSalePrice * item.Quantity,
                        };
                        cart.Add(createCartViewModel);
                    }
                }
                return cart;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
