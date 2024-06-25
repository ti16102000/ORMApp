namespace NHibernateApp.Models
{
    public class OrderItem : BaseEntity
    {
        public virtual Guid Id { get; set; }
        public virtual Guid OrderId { get; set; }
        public virtual string ProductSku { get; set; }
        public virtual decimal? ItemPrice { get; set; }
    }
}
