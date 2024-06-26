namespace NHibernateApp.Models
{
    public class Order : BaseEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
