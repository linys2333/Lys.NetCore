using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LysCore.Interfaces
{
    public interface IRepository<T, K> where T : class, IEntity<K>
    {
        Task<T> GetAsync(K id);

        Task<List<T>> GetListAsync();

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter);

        Task CreateAsync(T entity);

        Task CreateAsync(IEnumerable<T> entitys);

        Task UpdateAsync(T entity);

        Task UpdateAsync(IEnumerable<T> entitys);

        Task DeleteAsync(K id);

        Task DeleteAsync(IEnumerable<T> entitys);

        IQueryable<T> Where(Expression<Func<T, bool>> filter);
    }

    public interface IRepository<T> : IRepository<T, Guid> where T : class, IEntity<Guid>
    {
    }
}