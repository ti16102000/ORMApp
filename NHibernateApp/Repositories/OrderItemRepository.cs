using NHibernate.Linq;
using NHibernate.Util;
using NHibernateApp.Models;

namespace NHibernateApp.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        public async Task<bool> AddOrUpdateItemsAsync(List<OrderItem> orderItems, Guid orderId, bool isUpdateOrderSuccess = true)
        {
            if (!isUpdateOrderSuccess) return false;
            using (var session = NHibernateSession.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var orderItemIds = orderItems.Select(s => s.Id);
                    //get old order items by orderId
                    var oldOrderItems = await session.Query<OrderItem>().Where(a => a.OrderId == orderId).ToListAsync();
                    var oldOrderItemIds = oldOrderItems.Select(s => s.Id);

                    //delete old order items not exist
                    var delOldOrderItems = oldOrderItems.Where(a => !a.Id.Equals(orderItemIds)).ToList();
                    if (delOldOrderItems != null && delOldOrderItems.Any())
                    {
                        var delOldOrderItemIds = delOldOrderItems.Select(s => s.Id).ToList();
                        var delResult = await DeleteAsync(delOldOrderItemIds);
                        if(!delResult) return  false;
                    }                    
                    
                    //update old order items
                    var updateOldOrderItems = orderItems.Where(a => a.Id.Equals(oldOrderItemIds)).ToList();
                    if (updateOldOrderItems != null && updateOldOrderItems.Any()) {
                        foreach (var orderItem in updateOldOrderItems) {
                            await session.SaveAsync(orderItem);
                        }
                    }

                    //Add new order items
                    var newOrderItems = orderItems.Where(a => !a.Id.Equals(oldOrderItemIds)).ToList();
                    if(newOrderItems != null && newOrderItems.Any())
                    {
                        await session.SaveAsync(newOrderItems);
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
                    var resultDelOrderItems = await session.CreateQuery("DELETE TrainingOrderItem o WHERE o.Id IN (:guidIds)")
                                                            .SetParameterList("guidIds", guidIds)
                                                            .ExecuteUpdateAsync();
                    if (resultDelOrderItems < 1) return false;

                    transaction.Commit();
                    return true;
                }
            }               
        }

        public async Task<OrderItem> GetByIdAsync(Guid id)
        {
            using (var session = NHibernateSession.OpenSession())
            {
                var orderItem = await session.Query<OrderItem>().FirstOrDefaultAsync(a => a.Id == id);
                if (orderItem == null) return null;
                return orderItem;
            }
        }
        public async Task<List<OrderItem>> GetOrderItemsAsync(Guid? orderId, string orderNumber)
        {
            using (var session = NHibernateSession.OpenSession())
            {
                var orderItem = new List<OrderItem>();
                if (orderId != null && orderId != new Guid("{00000000-0000-0000-0000-000000000000}"))
                {
                    orderItem = await session.Query<OrderItem>().Where(a => a.OrderId == orderId).ToListAsync();
                }else if (!string.IsNullOrEmpty(orderNumber))
                {
                    var order = await session.Query<Order>().FirstOrDefaultAsync(a => a.OrderNumber.ToLower() == orderNumber.ToLower());
                    if (order == null) return null;
                    orderItem = await session.Query<OrderItem>().Where(a => a.OrderId == order.Id).ToListAsync();
                }                 
                return orderItem;
            }
        }
    }
}
