using EF006.ParameterlessConstructor.Data;
using EF006.ParameterlessConstructor.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



// // You want to execute job1 and job2 in parallel (concurrent) , so you have 3 solutions : 

// // Solution 1 : create DbContext for each task

// namespace EF006.ParameterlessConstructor
// {
//     class Program
//     {

//         static AppDbContext context = default!;
//         public static void Main()
//         {
//             var tasks = new Task[]
//             {
//                 Task.Factory.StartNew(() => Job1()),
//                 Task.Factory.StartNew(() => Job2())
//             };

//             Task.WhenAll(tasks).ContinueWith(t =>
//             {
//                 System.Console.WriteLine("Job1 and Job2 executed concurrently!");
//             });

//             Console.ReadKey();

//         }

//         static void Job1()
//         {

//             var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
//             var constr = configuration.GetSection("constr").Value;

//             var services = new ServiceCollection();
//             services.AddDbContext<AppDbContext>(options =>
//             {
//                 options.UseSqlServer(constr);
//             });

//             IServiceProvider serviceProvider = services.BuildServiceProvider();

//             context = serviceProvider.GetRequiredService<AppDbContext>();


//             var w1 = new Wallet
//             {
//                 Holder = "Brshama",
//                 Balance = 1000m
//             };

//             context.Wallets.Add(w1);
//             context.SaveChanges();
//         }

//         static void Job2()
//         {


//             var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
//             var constr = configuration.GetSection("constr").Value;

//             var services = new ServiceCollection();
//             services.AddDbContext<AppDbContext>(options =>
//             {
//                 options.UseSqlServer(constr);
//             });

//             IServiceProvider serviceProvider = services.BuildServiceProvider();

//             context = serviceProvider.GetRequiredService<AppDbContext>();

//             var w2 = new Wallet
//             {
//                 Holder = "Agsakhana",
//                 Balance = 1000m
//             };

//             context.Wallets.Add(w2);
//             context.SaveChanges();
//         }
//     }
// }






















// Soltuion 2 => by using await  +  async task


namespace EF006.ParameterlessConstructor
{
    class Program
    {

        static AppDbContext context = default!;
        public static void Main()
        {
            // Read Configuration
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var constr = configuration.GetSection("constr").Value;


            // Create DbContext using DI and IoC

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(constr);
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider(); // return IC Container 

            // create Factory to produce DbContext 
            context = serviceProvider.GetRequiredService<AppDbContext>();

            var tasks = new Task[]
            {
                Task.Factory.StartNew(() => Job1()),
                Task.Factory.StartNew(() => Job2())
            };

            Task.WhenAll(tasks).ContinueWith(t =>
            {
                System.Console.WriteLine("Job1 and Job2 executed concurrently!");
            });

            Console.ReadKey();

        }

        static async Task Job1()
        {
            var w1 = new Wallet
            {
                Holder = "Brshama",
                Balance = 1000m
            };

            context.Wallets.Add(w1);
            await context.SaveChangesAsync();
        }

        static async Task Job2()
        {
            var w2 = new Wallet
            {
                Holder = "Agsakhana",
                Balance = 1000m
            };

            context.Wallets.Add(w2);
            await context.SaveChangesAsync();
        }
    }
}