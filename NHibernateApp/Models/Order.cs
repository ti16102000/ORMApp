namespace NHibernateApp.Models
{
    public class Order : BaseEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
