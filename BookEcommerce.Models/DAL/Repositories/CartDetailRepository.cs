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
    public class CartDetailRepository : Repository<CartDetail>, ICartDetailRepository
    {
        public CartDetailRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<CartDetail> GetCartDetailByCartIdAndProductVariantId(Guid? cartId, Guid? productVariantId)
        {
            var req = await GetQuery(cd => cd.CartId == cartId && cd.ProductVariantId == productVariantId).AnyAsync();
            if (req)
            {
                return await GetQuery(cd => cd.CartId == cartId && cd.ProductVariantId == productVariantId).SingleAsync();
            }
            return null;
        }

        public async Task<List<CartDetail>> GetListCartDetailByCartId(Guid? cartId)
        {
            return await GetQuery(cd => cd.CartId.Equals(cartId)).ToListAsync();
        }
    }
}
