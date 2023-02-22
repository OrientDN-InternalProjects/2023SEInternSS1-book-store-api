using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
