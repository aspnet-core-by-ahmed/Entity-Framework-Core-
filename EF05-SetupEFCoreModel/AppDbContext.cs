using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MyApp.SetupEFCoreModel
{

    // Summary:
    //     A DbContext instance represents a session with the database and can be used to
    //     query and save instances of your entities. DbContext is a combination of the
    //     Unit Of Work and Repository patterns.

    // you can not use DbContext only because it can not know what is your entities !.
    // so , you must tell him entities + realtionships + other configurations 
    public class AppDbContext : DbContext
    {
        // Represent the collection of all entities 
        // for each table in databse you must have property 
        // by using DbSet<className>
        public DbSet<Wallet> Wallets { get; set; } = null!;
        // you tell to DbContext , I have entity his type is Wallet 

        // you must tell DbContext where is Database you will connect with 
        // like this : 
        // override method OnConfiguring
        // builder used to create or modify options for this context. and one of these options is Database 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var constr = configuration.GetSection("constr").Value;

            optionsBuilder.UseSqlServer(constr);
        }

    }
}