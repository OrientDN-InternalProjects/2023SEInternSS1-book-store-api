using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Request;
using BookEcommerce.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> AddOrder(OrderRequest orderRequest, Guid cusId);
        Task<OrderResponse> ChangeStatusOrder(StatusRequest statusReq, Guid orderId);
        Task<OrderViewModel> GetOrder(Guid orderId);
    }
}
