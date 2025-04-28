using Ecom.Application.Interfaces.Repositories;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var query = await _context.Set<T>().AsNoTracking().ToListAsync();

            return query;
        }


        //The order of includes matters for performance (larger collections should come last)
        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            return entity;
        }

        //var order = await repository.GetByIdAsync(
        //123,o => o.Customer,o => o.Items, o => o.Shipping);
        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            // Use EF.Property<TProperty>(instanceName,propertyName) to access the Id property dynamically
            var entity = await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);

            return entity;
        }

        public async Task<IReadOnlyList<T>> GetByConditionAsync(Expression<Func<T, bool>> condition)
        {
            var query = await _context.Set<T>().Where(condition).AsNoTracking().ToListAsync();

            return query;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

             _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
             _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }
    }
}
