using EF07.Configurations.Data.Config;
using EF07.Configurations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF07.Configurations.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var constr = config.GetRequiredSection("constr").Value;
            optionsBuilder.UseSqlServer(constr);



        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // way 1
            // individual call => hard if you have 100 configurations
            // new UserConfiguration().Configure(modelBuilder.Entity<User>());
            // new TweetConfiguration().Configure(modelBuilder.Entity<Tweet>());
            // new CommentConfiguration().Configure(modelBuilder.Entity<Comment>());


            // way 2 
            // call group configuration => easy if you have 100 configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }

    }
}