using EF006.ParameterlessConstructor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// DI Configuration 
// namespace EF006.ParameterlessConstructor
// {
//     class Program
//     {

//         public static void Main()
//         {
//             // Read Configuration
//             var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
//             var constr = configuration.GetSection("constr").Value;


//             // Create DbContext using DI and IoC

//             var services = new ServiceCollection();

//             services.AddDbContext<AppDbContext>(options =>
//             {
//                 options.UseSqlServer(constr);
//             });

//             IServiceProvider serviceProvider = services.BuildServiceProvider(); // return IC Container 

//             using (var context = serviceProvider.GetRequiredService<AppDbContext>())
//             {
//                 foreach (var wallet in context.Wallets)
//                 {
//                     System.Console.WriteLine(wallet);
//                 }
//             }
//         }
//     }
// }


// DbContext Factory Configuration 

namespace EF006.ParameterlessConstructor
{
    class Program
    {

        public static void Main()
        {
            // Read Configuration
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var constr = configuration.GetSection("constr").Value;


            // Create DbContext using DI and IoC

            var services = new ServiceCollection();

            services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseSqlServer(constr);
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider(); // return IC Container 

            // create Factory to produce DbContext 
            var contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

            using (var context = contextFactory.CreateDbContext())
            {
                foreach (var wallet in context.Wallets)
                {
                    System.Console.WriteLine(wallet);
                }
            }

            // you must dispose it by using Statement 
        }
    }
}