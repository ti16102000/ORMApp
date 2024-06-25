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
                    await session.SaveAsync(order);

                    //save order items
                    if(order.OrderItems != null && order.OrderItems.Any())
                    {
                        order.OrderItems.ForEach(item => {
                            item.OrderId = order.Id;
                        });
                        await session.SaveAsync(order.OrderItems);
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
                    //delete order
                    var resultDelOrder = await session.CreateQuery("DELETE TrainingOrder o WHERE o.Id IN (:guidIds)")
                                                        .SetParameterList("guidIds", guidIds)
                                                        .ExecuteUpdateAsync();
                    if (resultDelOrder < 1) return false;

                    //delete order items by orderIds
                    var resultDelOrderItems = await session.CreateQuery("DELETE TrainingOrderItem o WHERE o.Order_ID_FK IN (:guidIds)")
                                                    .SetParameterList("guidIds", guidIds)
                                                    .ExecuteUpdateAsync();
                    if (resultDelOrderItems < 1) return false;

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

                if (isLoadItems)
                {
                    var orderItems = await session.Query<OrderItem>().Where(a => a.OrderId == id).ToListAsync();
                    order.OrderItems = orderItems;
                }
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
                var orders = await session.Query<Order>().ToListAsync();
                return orders;
            }
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            using (var session = NHibernateSession.OpenSession())
            {
                var existOrder = await session.Query<Order>().AnyAsync(a => a.Id == order.Id);
                if (existOrder) return null;
                using (var transaction = session.BeginTransaction())
                {
                    await session.UpdateAsync(order);
                    transaction.Commit();
                    return order;
                }
            }
        }
    }
}
