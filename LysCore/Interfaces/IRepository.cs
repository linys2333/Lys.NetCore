using System;
using System.Threading.Tasks;

namespace LysCore.Interfaces
{
    public interface IRepository<T, K> where T : class, IEntity<K>
    {
        Task<T> GetAsync(K id);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(K id);
    }

    public interface IRepository<T> : IRepository<T, Guid> where T : class, IEntity<Guid>
    {
    }
}