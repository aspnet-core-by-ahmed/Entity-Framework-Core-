using EF09.EntityTypesAndMapping.Entities;

namespace EF09_EntityTypesAndMapping.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public string Description { get; set; } = null!;
        public Snapshot Snapshot { get; } = new Snapshot();
    }
}