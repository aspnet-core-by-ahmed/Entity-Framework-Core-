namespace EF09_EntityTypesAndMapping.Entities
{
    public class OrderBill
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public override string ToString()
        {
            return $"{OrderId} at {OrderDate} and Name: {ProductName}\nPrice: {UnitPrice} - \tQuantity: {Quantity}\n......{SubTotal:C}";
        }
    }
}