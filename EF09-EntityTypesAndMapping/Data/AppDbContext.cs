using System.Runtime.Intrinsics.Arm;
using EF09.EntityTypesAndMapping.Entities;
using EF09_EntityTypesAndMapping.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF09.EntityTypesAndMapping.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderWithDetailsView> OrderWithDetails { get; set; }
        public DbSet<OrderBill> OrderGivenBill { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var constr = configuration.GetRequiredSection("constr").Value;
            optionsBuilder.UseSqlServer(constr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // each table follow schema 
            modelBuilder.Entity<Product>()
                .ToTable("Products", schema: "Inventory")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Order>()

                .HasKey(o => o.Id);

            modelBuilder.Entity<OrderDetail>()
                .ToTable("OrderDetails", schema: "Sales")
                .HasKey(od => od.Id);

            // here he will decide that this is default schema for all tables if not specified (override)
            // modelBuilder.HasDefaultSchema("Sales");

            modelBuilder.Entity<AuditEntry>().HasKey(ae => ae.Id);

            modelBuilder.Entity<OrderWithDetailsView>()
                .HasNoKey()
                .ToView("OrderWithDetailsView"); // default schema is dbo, if not specified in the view creation


            modelBuilder.Entity<OrderBill>()
                .ToFunction("GetOrderBill")  // Maps to the TVF name
                .HasNoKey();  // No PK needed

            base.OnModelCreating(modelBuilder);
        }
    }
}