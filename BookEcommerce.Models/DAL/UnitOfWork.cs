using BookEcommerce.Models.DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbFactory dbFactory;
        public UnitOfWork(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
        public Task CommitTransaction()
        {
            return dbFactory.DbContext.SaveChangesAsync();
        }
    }
}
