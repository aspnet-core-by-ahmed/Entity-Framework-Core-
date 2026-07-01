using EF09.EntityTypesAndMapping.Entities;

namespace EF09_EntityTypesAndMapping.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Snapshot Snapshot { get; } = new Snapshot();

    }
}