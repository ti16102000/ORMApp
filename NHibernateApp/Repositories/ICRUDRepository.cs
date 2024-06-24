using NHibernateApp.Models;

namespace NHibernateApp.Repositories
{
    public interface ICRUDRepository<T> where T : BaseEntity
    {
        Task<T> AddOrUpdateAsync(T entity);
        void DeleteAsync(string entityId);
        Task<IEnumerable<T>> GetAllAsync(GetRequest<T>? request);
        Task<T>? GetByIdAsync(string entityId);
    }
}
