using NHibernateApp.Models;

namespace NHibernateApp.Repositories
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetOrderItemsAsync(Guid? orderId, string orderNumber = "");
        Task<bool> AddOrUpdateItemsAsync(List<OrderItem> orderItems, Guid orderId, bool isUpdateOrderSuccess = true);
        Task<OrderItem> GetByIdAsync(Guid id);
        Task<OrderItem?> Update(OrderItem orderItem);
        Task<bool> DeleteAsync(List<Guid> guidIds);
    }
}
