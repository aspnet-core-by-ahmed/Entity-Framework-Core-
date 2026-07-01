using EF006.ParameterlessConstructor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace EF006.ParameterlessConstructor.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }

        // way 1 
        // External Configuration 
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     base.OnConfiguring(optionsBuilder);
        //     var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        //     var constr = configuration.GetSection("constr").Value;
        //     optionsBuilder.UseSqlServer(constr);
        // }


        // way 2 
        // Internal Configuration 
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}