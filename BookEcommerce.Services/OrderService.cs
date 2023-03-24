using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Base;
using BookEcommerce.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookEcommerce.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly IProductVariantRepository productVariantRepository;
        private readonly IProductPriceRepository productPriceRepository;
        private readonly IProductRepository productRepository;
        private readonly ICartDetailRepository cartDetailRepository;
        private readonly ICartRepository cartRepository;
        private readonly ICartDetailService cartDetailService;
        private readonly ITokenRepository tokenRepository;
        private readonly ILogger<OrderService> logger;
        public OrderService(IUnitOfWork unitOfWork, 
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IProductVariantRepository productVariantRepository, 
            IProductPriceRepository productPriceRepository, 
            IProductRepository productRepository,
            ICartDetailRepository cartDetailRepository, 
            ICartRepository cartRepository, 
            ILogger<OrderService> logger, 
            ICustomerRepository customerRepository,
            ITokenRepository tokenRepository,
            ICartDetailService cartDetailService) : base(unitOfWork)
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
            this.customerRepository = customerRepository;
            this.tokenRepository = tokenRepository;
        }

        public async Task<OrderResponse> AddOrder(OrderRequest req, Guid cusId)
        {
            try
            {
                var order = await createNewOrder(req, cusId);
                var res = await addOrderDetails(req,order);
                if (!res.IsSuccess) return res;
                await deleteProductInCart(order, req);
                await _unitOfWork.CommitTransaction();
                return new OrderResponse
                {
                    IsSuccess = true,
                    Message = "Add order success! " ,
                    Link = "https://localhost:7018/api/order/" + order.OrderId.ToString()
                };
            }
            catch (InvalidOperationException e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "Some properties is valid !",
                };
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
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
                if (statusReq.StatusOrder!.Equals("Cancel"))
                {
                    findOrder.StatusOrder = statusReq.StatusOrder;
                    var findOrderDetail = await orderDetailRepository.GetOrderDetailsByOrderId(orderId);
                    foreach (var item in findOrderDetail)
                    {
                        var findProductVariant = await productVariantRepository.GetProductVariantById(item.ProductVariantId); 
                        findProductVariant.Quantity += item.Quantity;
                        findProductVariant.Product!.Sold -= item.Quantity;
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
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "Some properties is Null!"
                };
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
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
                        ProductName = findProduct.ProductVariantName!,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    listOrderDetails.Add(orderDetail);
                }
                return new OrderViewModel
                { 
                    OrderId = orderId,
                    IsSuccess = true,
                    CustomerId = findOrder.CustomerId!.Value,
                    TransferAddress = findOrder.TransferAddress!,
                    Message = findOrder.Message!,
                    OrderDate = findOrder.OrderDate,
                    OrderStatus = findOrder.StatusOrder!,
                    TotalPrice = findOrder.TotalPrice,
                    OrderDetails = listOrderDetails
                };
            }
            catch (NullReferenceException e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new OrderViewModel
                {
                    IsSuccess = false,
                    Message = "Some properties is Null! "
                };
            }
            catch (InvalidOperationException e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new OrderViewModel
                { 
                    IsSuccess = false,
                    Message = "Can't find your Order in our System! "
                };
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}. Detail {e.StackTrace}");
                return new OrderViewModel
                {
                    IsSuccess = false,
                    Message = "System error, Add Order was false and please try again later! "
                };
            }
        }
        private async Task<Order> createNewOrder(OrderRequest req, Guid cusId)
        {
            var order = new Order
            {
                CustomerId = cusId,
                TransferAddress = req.TransferAddress,
                PaymentId = req.PaymentMethodId,
                Message = req.Message,
                OrderDate = DateTime.Now,
                StatusOrder = "Pending",
                TotalPrice = 0
            };
            await orderRepository.AddAsync(order);
            return order;
        }

        private async Task<OrderResponse> addOrderDetails(OrderRequest req, Order order)
        {
            foreach (var item in req.Details)
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
                findProductVariant.Product!.Sold += item.Quantity;
                if (findProductVariant.Quantity < 0)
                {
                    logger.LogError("Can't order more than available quantity!");
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message = "Can't order more than available quantity!"
                    };
                }
                productVariantRepository.Update(findProductVariant);
            }
            return new OrderResponse
            {
                IsSuccess = true
            };
        }

        private async Task deleteProductInCart(Order order, OrderRequest req)
        {
            foreach (var orde in req.Details)
            {
                var findCart = await cartRepository.GetCartByCustomerId(order.CustomerId);
                var findCartDetail = await cartDetailRepository.GetCartDetailByCartIdAndProductVariantId(findCart.CartId, orde.ProductVariantId);
                if (findCartDetail != null)
                {
                    cartDetailRepository.Delete(findCartDetail);
                }
            }
        }

        public async Task<OrderViewModel> GetOrderByCustomerId(string token)
        {
            var userId = this.tokenRepository.GetUserIdFromToken(token);
            var customer = await this.customerRepository.FindAsync(v => v.AccountId!.Equals(userId.ToString()));
            var findOrder = await orderRepository.GetOrderByCustomerId(customer.CustomerId);
            var findOrderDetails = await orderDetailRepository.GetOrderDetailsByOrderId((Guid)findOrder.OrderId!);
            var listOrderDetails = new List<OrderDetailViewModel>();
            foreach (var item in findOrderDetails)
            {
                var findProduct = await productVariantRepository.GetProductVariantById(item.ProductVariantId);
                var orderDetail = new OrderDetailViewModel
                {
                    ProductVariantId = item.ProductVariantId,
                    ProductName = findProduct.ProductVariantName!,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                listOrderDetails.Add(orderDetail);
            }
            return new OrderViewModel
            {
                OrderId = (Guid)findOrder.OrderId,
                IsSuccess = true,
                CustomerId = findOrder.CustomerId!.Value,
                TransferAddress = findOrder.TransferAddress!,
                Message = findOrder.Message!,
                OrderDate = findOrder.OrderDate,
                OrderStatus = findOrder.StatusOrder!,
                TotalPrice = findOrder.TotalPrice,
                OrderDetails = listOrderDetails
            };
        }
    }
}
