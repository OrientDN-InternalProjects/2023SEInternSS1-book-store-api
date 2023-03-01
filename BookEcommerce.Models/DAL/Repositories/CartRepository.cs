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
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }


        public async Task<Cart> GetCartByCustomerId(Guid id)
        {
            return await GetQuery(crt => crt.CustomerId.Equals(id)).SingleAsync();
        }
    }
}
