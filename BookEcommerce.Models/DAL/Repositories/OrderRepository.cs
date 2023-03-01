using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<Order> GetOrderByOrderId(Guid orderId)
        {
            return await GetQuery(or => or.OrderId == orderId).SingleAsync();
        }
    }
}
