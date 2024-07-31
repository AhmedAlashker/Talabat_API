using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Sepecifications;
using Talabat.Infrastructure._Data;

namespace Talabat.Infrastructure.Generic_Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
			if (typeof(T) == typeof(Product))
				return (IReadOnlyList<T>)await _dbContext.Set<Product>().Include(P => P.Brand).Include(P => P.Category).ToListAsync();

			return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            //if (typeof(T) == typeof(Product))
            //    return await _dbContext.Set<Product>().Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;

            return await _dbContext.Set<T>().FindAsync(id);
        }

        // 
        #region After Using Specifications Pattern

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISepecifications<T> spec)
        {
            return await ApplySepecifications(spec).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdWithSpecAsync(ISepecifications<T> spec)
        {
            return await ApplySepecifications(spec).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISepecifications<T> spec)
        {
            return await ApplySepecifications(spec).CountAsync();
        }

        private IQueryable<T> ApplySepecifications(ISepecifications<T> spec)
        {
            return SepecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        #endregion


        public void Add(T entity)
            => _dbContext.Set<T>().Add(entity);

        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);
    }
}
