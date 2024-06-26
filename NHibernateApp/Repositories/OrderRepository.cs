using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using NHibernateApp.Models;

namespace NHibernateApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<bool> AddOrderAndItemsAsync(Order order)
        {
            using (var session = NHibernateSession.OpenSession())
            {
                var existOrder = await session.Query<Order>().AnyAsync(a => a.OrderNumber.ToLower().Equals(order.OrderNumber.ToLower()));
                if (existOrder) return false;
                using (var transaction = session.BeginTransaction())
                {
                    await session.SaveOrUpdateAsync(order);
                    foreach (var item in order.OrderItems)
                    {
                        item.Order = order;
                        await session.SaveOrUpdateAsync(item);
                    }
                    transaction.Commit();
                    return true;
                }
            }
        }

        public async Task<bool> DeleteAsync(List<Guid> guidIds)
        {
            using (var session = NHibernateSession.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var orders = await session.Query<Order>().Where(a => guidIds.Contains(a.Id)).ToListAsync();
                    foreach (var item in orders)
                    {
                        await session.DeleteAsync(item);
                    }

                    transaction.Commit();
                    return true;
                }
            }               
        }

        public async Task<Order?> GetByIdAsync(Guid id, bool isLoadItems = false)
        {
            using (var session = NHibernateSession.OpenSession())
            {
                var order = await session.Query<Order>().FirstOrDefaultAsync(a => a.Id == id);
                if (order == null) return null;
                return order;
            }
        }

        public async Task<Order> GetByOrderNumberAsync(string orderNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            using (var session = NHibernateSession.OpenSession())
            {
               var orders = await session.Query<Order>().Fetch(o => o.OrderItems).ToListAsync();
                return orders;
            }
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            using (var session = NHibernateSession.OpenSession())
            {
                var existOrder = await session.Query<Order>().AnyAsync(a => a.Id == order.Id);
                if (!existOrder) return null;
                using (var transaction = session.BeginTransaction())
                {
                    await session.MergeAsync(order);
                    transaction.Commit();
                    return order;
                }
            }
        }
    }
}
