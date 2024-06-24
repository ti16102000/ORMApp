using NHibernate;
using NHibernate.Linq;
using NHibernateApp.Models;
using System.Transactions;

namespace NHibernateApp.Repositories
{
    public class CRUDRepository<T> : ICRUDRepository<T> where T : BaseEntity
    {

        public CRUDRepository()
        {

        }

        public async Task<T> AddOrUpdateAsync(T entity)
        {
            using (NHibernate.ISession session = NHibernateSession.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    await session.SaveAsync(entity);
                    transaction.Commit();
                }
            }
            return entity;
        }

        public async void DeleteAsync(string entityId)
        {
            using (NHibernate.ISession session = NHibernateSession.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var entity = await session.GetAsync<T>(entityId);
                    if (entity != null) await session.DeleteAsync(entityId);
                    
                    transaction.Commit();
                }
                    
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(GetRequest<T> request)
        {
            using (NHibernate.ISession session = NHibernateSession.OpenSession())
            {
                var query = session.Query<T>();
                if (request.Filter != null)
                {
                    query = query.Where(request.Filter);
                }

                if (request.OrderBy != null)
                {
                    query = request.OrderBy(query);
                }

                if (request.Skip.HasValue)
                {
                    query = query.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    query = query.Take(request.Take.Value);
                }
                return await query.ToListAsync();
            }
            
        }

        public async Task<T>? GetByIdAsync(string entityId)
        {
            using (NHibernate.ISession session = NHibernateSession.OpenSession())
            {
                return await session.GetAsync<T>(entityId);
            }            
        }
    }
}
