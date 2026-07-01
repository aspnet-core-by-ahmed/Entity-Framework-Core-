namespace EF09.EntityTypesAndMapping.Entities
{
    public class OrderWithDetailsView
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerEmail { get; set; } = null!;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return @$"OrderId: {OrderId}, OrderDate: {OrderDate}, CustomerEmail: {CustomerEmail},
                    ProductId: {ProductId}, ProductName: {ProductName}, UnitPrice: {UnitPrice}, Quantity: {Quantity}";
        }
    }
}


