﻿using BookEcommerce.Models.DAL.Interfaces;
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
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly IProductPriceRepository productPriceRepository;
        private readonly IProductRepository productRepository;
        private readonly ICartDetailRepository cartDetailRepository;
        private readonly ICartRepository cartRepository;
        private readonly ICartDetailService cartDetailService;
        private readonly ILogger<OrderService> logger;
        public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository,IOrderDetailRepository orderDetailRepository,
                            IProductVariantRepository productVariantRepository, IProductPriceRepository productPriceRepository, IProductRepository productRepository,
                            ICartDetailRepository cartDetailRepository, ICartRepository cartRepository, ILogger<OrderService> logger, ICartDetailService cartDetailService) : base(unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.productVariantRepository = productVariantRepository;
            this.productPriceRepository = productPriceRepository;
            this.productRepository = productRepository;
            this.cartDetailRepository = cartDetailRepository;
            this.cartRepository = cartRepository;
            this.logger = logger;
            this.cartDetailService = cartDetailService;
        }
        public async Task<OrderResponse> AddOrder(OrderRequest req, Guid cusId)
        {
            try
            {
                var order = new Order
                {
                    CustomerId = cusId,
                    TransferAddress = req.TransferAddress,
                    PaymentId = req.PaymentId,
                    Message = req.Message,
                    OrderDate = DateTime.Now,
                    StatusOrder = "Pending",
                    VendorId = req.ShopId,
                    TotalPrice = 0
                };
                await orderRepository.AddAsync(order);
                foreach (var item in req.Details!)
                {
                    var priceProduct = await productPriceRepository.GetPriceByProductVariant(item.ProductVariantId);
                    var findProductVariant = await productVariantRepository.GetProductVariantById(item.ProductVariantId);
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductVariantId = item.ProductVariantId,
                        Quantity = item.Quantity,
                        Price = ((double)priceProduct) * item.Quantity
                    };
                    await orderDetailRepository.AddAsync(orderDetail);
                    order.TotalPrice += orderDetail.Price;
                    findProductVariant.Quantity -= item.Quantity;
                    if(findProductVariant.Quantity < 0)
                    {
                        logger.LogError("Can't order more than avilable Quantity!");
                        return new OrderResponse
                        {
                            IsSuccess = false,
                            Message = "Can't order more than avilable Quantity!"
                        };
                    }
                    productVariantRepository.Update(findProductVariant);
                    //await _unitOfWork.CommitTransaction();
                }
                //await _unitOfWork.CommitTransaction();
                foreach (var orde in req.Details)
                {
                    var findCart = await cartRepository.GetCartByCustomerId(order.CustomerId);
                    var findCartDetail = await cartDetailRepository.GetCartDetailByCartIdAndProductVariantId(findCart.CartId, orde.ProductVariantId);
                    if (findCartDetail != null)
                    {
                        cartDetailRepository.Delete(findCartDetail);
                    }
                }
                await _unitOfWork.CommitTransaction();
                return new OrderResponse
                {
                    IsSuccess = true,
                    Message = "Add Order Success and you can view Product in Link: https://localhost:7018/api/order/" + order.OrderId.ToString()
                };
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace);
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "Some properties is valid !",
                };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace);
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "System error, please try again later!"
                };
            }
        }

        public async Task<OrderResponse> ChangeStatusOrder(StatusRequest statusReq, Guid orderId)
        {
            try
            {
                var findOrder = await orderRepository.GetOrderByOrderId(orderId);
                if (statusReq.StatusOrder.Equals("Cancel"))
                {
                    findOrder.StatusOrder = statusReq.StatusOrder;
                    var findOrderDetail = await orderDetailRepository.GetOrderDetailsByOrderId(orderId);
                    foreach (var item in findOrderDetail)
                    {
                        var findProductVariant = await productVariantRepository.GetProductVariantById(item.ProductVariantId);
                        findProductVariant.Quantity += item.Quantity;
                        productVariantRepository.Update(findProductVariant);
                    }
                }
                else
                {
                    findOrder.StatusOrder = statusReq.StatusOrder;
                    orderRepository.Update(findOrder);
                }
                await _unitOfWork.CommitTransaction();
                return new OrderResponse 
                { 
                    IsSuccess = true,
                    Message = "Change Status Order is success!"
                };
            }
            catch (NullReferenceException e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace);
                return new OrderResponse 
                { 
                    IsSuccess = false,
                    Message = "Some properties is Null!"
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace);
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "System error, please try again later!"
                };
            }
        }

        public async Task<OrderViewModel> GetOrder(Guid orderId)
        {
            try
            { 
                var findOrder = await orderRepository.GetOrderByOrderId(orderId);
                var findOrderDetails = await orderDetailRepository.GetOrderDetailsByOrderId(orderId);
                var listOrderDetails = new List<OrderDetailViewModel>();
                foreach (var item in findOrderDetails)
                {
                    var findProduct = await productVariantRepository.GetProductVariantById(item.ProductVariantId);
                    var orderDetail = new OrderDetailViewModel
                    {
                        ProductVariantId = item.ProductVariantId,
                        ProductName = findProduct.ProductVariantName,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    listOrderDetails.Add(orderDetail);
                }
                return new OrderViewModel
                {
                    IsSuccess = true,
                    TransferAddress = findOrder.TransferAddress,
                    Message = findOrder.Message,
                    OrderDate = findOrder.OrderDate,
                    OrderStatus = findOrder.StatusOrder,
                    TotalPrice = findOrder.TotalPrice,
                    OrderDetails = listOrderDetails
                };
            }
            catch (NullReferenceException e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace);
                return new OrderViewModel
                {
                    IsSuccess = false,
                    Message = "Some properties is Null! "
                };
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace);
                return new OrderViewModel
                {
                    IsSuccess = false,
                    Message = "Can't find your Order in our System! "
                };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message + "\n" + e.StackTrace);
                return new OrderViewModel
                {
                    IsSuccess = false,
                    Message = "System error, Add Product was fasle and please try again later! "
                };
            }
        }
    }
}
