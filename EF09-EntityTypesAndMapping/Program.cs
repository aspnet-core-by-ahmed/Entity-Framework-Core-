using EF09.EntityTypesAndMapping.Data;
using Microsoft.EntityFrameworkCore;

namespace EF09.EntityTypesAndMapping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter Number , (1) for Products , (2) for View OrderWithDetailsView , (3) for TVF OrderBill: ");
            int choice = int.Parse(Console.ReadLine() ?? "1");

            if (choice == 1)
            {
                using (var context = new AppDbContext())
                {
                    foreach (var product in context.Products)
                    {
                        Console.WriteLine(@$"Product: {product.Name}, Price: {product.UnitPrice},
                        LoadedAt: {product.Snapshot.LoadedAt.ToString("yyyy-mm-dd hh:mm:ss")}, Version: {product.Snapshot.Version}");
                    }
                }
            }
            else if (choice == 2)
            {
                using (var context = new AppDbContext())
                {
                    foreach (var orderWithDetails in context.OrderWithDetails)
                    {
                        Console.WriteLine(orderWithDetails.ToString());
                    }
                }
            }
            else if (choice == 3)
            {
                using (var context = new AppDbContext())
                {
                    var orderId = 1; // Replace with the desired order ID
                    var orderBill = context.OrderGivenBill.FromSqlRaw("SELECT * FROM dbo.GetOrderBill({0})", orderId).ToList();

                    foreach (var bill in orderBill)
                    {
                        System.Console.WriteLine(bill);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }

        }
    }
}