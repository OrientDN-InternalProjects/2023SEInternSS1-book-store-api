using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL
{
    public class DbFactory : IDisposable
    {
        private bool disposed;
        private Func<ApplicationDbContext> instanceFunc;
        private ApplicationDbContext dbContext;
        public ApplicationDbContext DbContext => dbContext ?? (dbContext = instanceFunc.Invoke());
        public DbFactory(Func<ApplicationDbContext> dbContextFactory)
        {
            instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!disposed && dbContext != null)
            {
                disposed = true;
                dbContext.Dispose();
            }
        }
    }
}
