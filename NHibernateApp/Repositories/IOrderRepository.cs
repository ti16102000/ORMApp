using NHibernateApp.Models;

namespace NHibernateApp.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync();
        Task<bool> AddOrderAndItemsAsync(Order order);
        Task<Order?> GetByIdAsync(Guid id, bool isLoadItems = false);
        Task<Order> GetByOrderNumberAsync(string orderNumber);
        Task<Order?> UpdateAsync(Order order);
        Task<bool> DeleteAsync(List<Guid> guidIds);
    }
}
