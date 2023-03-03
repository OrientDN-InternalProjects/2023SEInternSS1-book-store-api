using BookEcommerce.Models.DAL.Interfaces;
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
                var orderLoading = new Order();
                foreach (var item in req.Details)
                {
                    var order = new Order
                    {
                        CustomerId = cusId,
                        TransferAddress = req.TransferAddress,
                        PaymentId = req.PaymentId,
                        Message = req.Message,
                        OrderDate = DateTime.Now,
                        StatusOrder = "Pending",
                        VendorId = item.ShopId,
                        TotalPrice = 0
                    };
                    await orderRepository.AddAsync(order);
                    foreach (var ordt in item.OrderDetailRequests)
                    {
                        var priceProduct = await productPriceRepository.GetPriceByProductVariant(ordt.ProductVariantId);
                        var findProductVariant = await productVariantRepository.GetProductVariantById(ordt.ProductVariantId);
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductVariantId = ordt.ProductVariantId,
                            Quantity = ordt.Quantity,
                            Price = ((double)priceProduct) * ordt.Quantity
                        };
                        await orderDetailRepository.AddAsync(orderDetail);
                        order.TotalPrice += orderDetail.Price;
                        findProductVariant.Quantity -= ordt.Quantity;
                        if(findProductVariant.Quantity < 0)
                        {
                            return new OrderResponse
                            {
                                IsSuccess = false,
                                Message = "Can't order more than avilable Quantity!"
                            };
                        }
                        productVariantRepository.Update(findProductVariant);
                    }
                    await _unitOfWork.CommitTransaction();
                    orderRepository.Update(order);
                    orderLoading = order;
                }
                await _unitOfWork.CommitTransaction();
                var findOrder = await orderRepository.GetOrderByOrderId(orderLoading.OrderId.Value);
                var listOrderDetails = await orderDetailRepository.GetOrderDetailsByOrderId(orderLoading.OrderId.Value);
                foreach (var or in listOrderDetails)
                {
                    var findCart = cartRepository.GetCartByCustomerId(findOrder.CustomerId);
                    var findCartDetail = await cartDetailRepository.GetCartDetailByCartIdAndProductVariantId(findCart.Result.CartId, or.ProductVariantId);
                    cartDetailRepository.Delete(findCartDetail);
                }
                await _unitOfWork.CommitTransaction();
                return new OrderResponse
                {
                    IsSuccess = true
                };
            }
            catch (InvalidOperationException)
            {
                logger.LogError("Some properties is valid when Customer Add Order! ");
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "Some properties is valid !",
                };
            }
            catch(Exception)
            {
                logger.LogError("System error and Exception was not found when Customer Add Order! ");
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "System error, please try again later!"
                };
            }
        }

        public async Task<OrderResponse> CancelOrder(Guid orderId)
        {
            try
            {
                var findOrder = await orderRepository.GetOrderByOrderId(orderId);
                findOrder.StatusOrder = "Cancel";
                var findOrderDetail = await orderDetailRepository.GetOrderDetailsByOrderId(orderId);
                foreach (var item in findOrderDetail)
                {
                    var findProductVariant = await productVariantRepository.GetProductVariantById(item.ProductVariantId); 
                    findProductVariant.Quantity += item.Quantity;
                    productVariantRepository.Update(findProductVariant);
                }
                await _unitOfWork.CommitTransaction();
                return new OrderResponse
                {
                    IsSuccess = true,
                    Message = "Cancel Order is success! "
                };
            }
            catch (InvalidOperationException)
            {
                logger.LogError("Some properties is valid when Cancel Order! ");
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "Some propaties is valid !",
                };
            }
            catch (Exception)
            {
                logger.LogError("System error and Exception was not found when Cancel Order! ");
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "System error, Cancel Order was fasle and please try again later! "
                };
            }
        }

        public async Task<OrderResponse> ChangeStatusOrder(StatusRequest statusReq, Guid orderId)
        {
            try
            {
                var findOrder = await orderRepository.GetOrderByOrderId(orderId);
                findOrder.StatusOrder = statusReq.StatusOrder;
                orderRepository.Update(findOrder);
                await _unitOfWork.CommitTransaction();
                return new OrderResponse 
                { 
                    IsSuccess = true,
                    Message = "Change Status Order is success!"
                };
            }
            catch (NullReferenceException)
            {
                logger.LogError("Some properties is Null when Vendor Update Order! ");
                return new OrderResponse 
                { 
                    IsSuccess = false,
                    Message = "Some properties is Null!"
                };
            }
            catch (Exception)
            {
                logger.LogError("System error and Exception was not found when Vendor Change Order! ");
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "System error, please try again later!"
                };
            }
        }
    }
}
