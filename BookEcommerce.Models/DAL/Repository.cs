using BookEcommerce.Models.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbFactory dbFactory;
        private DbSet<T> dbSet;

        private DbSet<T> DbSet
        {
            get => dbSet ?? ( dbSet = dbFactory.DbContext.Set<T>());
        }
        public Repository(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public Repository()
        {
        }

        public async Task<T> AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }
        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await DbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public IQueryable<T> GetQuery(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }
    }
}
