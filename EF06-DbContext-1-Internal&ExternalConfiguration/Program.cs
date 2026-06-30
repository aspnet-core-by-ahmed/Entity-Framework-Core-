using EF006.ParameterlessConstructor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF006.ParameterlessConstructor
{
    class Program
    {

        public static void Main()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var constr = configuration.GetSection("constr").Value;

            // var options = new DbContextOptions(); // becuase DbContextOptions is an abstract class

            // so you can use Builder pattern and factory method
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(constr);

            var options = optionsBuilder.Options;

            using (var context = new AppDbContext(options))
            {
                foreach (var wallet in context.Wallets)
                {
                    System.Console.WriteLine(wallet);
                }
            }
        }
    }
}