using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lys.NetCore.Infrastructure.Entities
{
    public interface IRepository<T, K> where T : class, IEntity<K>
    {
        Task<T> GetAsync(K id);

        Task<List<T>> GetListAsync();

        Task CreateAsync(T entity);

        Task CreateAsync(IEnumerable<T> entitys);

        Task UpdateAsync(T entity);

        Task UpdateAsync(IEnumerable<T> entitys);

        Task DeleteAsync(K id);

        Task DeleteAsync(IEnumerable<T> entitys);

        IQueryable<T> Where(Expression<Func<T, bool>> filter);

        IQueryable<T> WhereIf(Expression<Func<T, bool>> filter, bool condition);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    }

    public interface IRepository<T> : IRepository<T, Guid> where T : class, IEntity<Guid>
    {
    }
}