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
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Image>> GetImagesByProductId(Guid productId)
        {
            return await GetQuery(pi => pi.ProductId.Equals(productId)).ToListAsync();
        }
    }
}
