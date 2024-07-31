using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Sepecifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISepecifications<T> spec);


        Task<T?> GetByIdWithSpecAsync(ISepecifications<T> spec);

        Task<int> GetCountAsync(ISepecifications<T> spec);


        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
