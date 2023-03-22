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
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<Address>> GetAddressByCusId(Guid cusId)
        {
            return await GetQuery(p => p.CustomerId!.Value == cusId).ToListAsync();
        }
    }
}
