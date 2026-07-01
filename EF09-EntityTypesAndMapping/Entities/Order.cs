using EF09.EntityTypesAndMapping.Entities;

namespace EF09_EntityTypesAndMapping.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public Snapshot Snapshot { get; } = new Snapshot();

    }
}