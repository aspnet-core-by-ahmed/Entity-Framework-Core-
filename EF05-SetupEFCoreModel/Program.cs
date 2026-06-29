
using System.Runtime.InteropServices;

namespace MyApp.SetupEFCoreModel
{
    class Program
    {
        public static void Main()
        {
            // Retrieve Data
            System.Console.WriteLine("--------------- Retrieve Data ---------------");
            using (var context = new AppDbContext())
            {
                // if you go inside DbSet will see that it is implement IQueryable
                //  and inside IQuerable you will see that it implement IEnumerable so you can use foreach
                foreach (var wallet in context.Wallets)
                {
                    Console.WriteLine(wallet);
                }
            }

            System.Console.WriteLine("------------ Retrieve Single Item ------------");
            var itemIdToRetrieve = 2;
            using (var context = new AppDbContext())
            {
                var item = context.Wallets.FirstOrDefault(x => x.Id == itemIdToRetrieve);
                System.Console.WriteLine(item);
            }

            System.Console.WriteLine("----------------- Insert Data ------------------");
            var newWallet = new Wallet
            {
                Holder = "Sayed",
                Balance = 1200m
            };

            using (var context = new AppDbContext())
            {

                context.Wallets.Add(newWallet); // insertion happen in memory 
                context.SaveChanges(); // to make insertion happen in database 
                System.Console.WriteLine("inserted successfully");
            }

            System.Console.WriteLine("----------------- Update Data ------------------");
            using (var context = new AppDbContext())
            {
                try
                {
                    // update wallet id = 3 increase balance by 1000
                    int targetId = 3;
                    var wallet = context.Wallets.SingleOrDefault(w => w.Id == targetId);
                    wallet?.Balance += 1000;
                    context.SaveChanges();
                    System.Console.WriteLine($"wallet with id = '{targetId}' updated successfully");

                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }

            System.Console.WriteLine("----------------- Delete Data ------------------");
            using (var context = new AppDbContext())
            {
                // delete wallet with Id = 6 or 5 
                var wallet1 = context.Wallets.SingleOrDefault(x => x.Id == 5);
                var wallet2 = context.Wallets.SingleOrDefault(x => x.Id == 6);
                context.Wallets.Remove(wallet1!);
                context.Wallets.Remove(wallet2!);
                context.SaveChanges();
                System.Console.WriteLine("Delete SuccessFully");
            }

            System.Console.WriteLine("------------- Quering Data ------------");
            using (var context = new AppDbContext())
            {
                // retrieve all wallets with balance over 5000
                var result = context.Wallets.Where(x => x.Balance > 5000);
                foreach (var wallet in result)
                {
                    System.Console.WriteLine(wallet);
                }
            }


            System.Console.WriteLine("------------ Implement Transactions ------------");
            // collection of operations done all or not happen
            using (var context = new AppDbContext())
            {
                // to enable transaction / note : transaction object implement IDisposibal
                using (var transaction = context.Database.BeginTransaction())
                {
                    // transfer 1000$ from wallet with id = 2 to wallet with id = 1
                    var wallet1 = context.Wallets.Single(x => x.Id == 2);
                    var wallet2 = context.Wallets.Single(x => x.Id == 1);
                    var amountToTransfer = 1000;

                    // withdraw from wallet 1 
                    wallet1.Balance -= amountToTransfer;
                    context.SaveChanges();

                    // Deposit from wallet 2
                    wallet2.Balance += amountToTransfer;
                    context.SaveChanges();

                    transaction.Commit(); // if you comment it , execution will not be happen 
                    // you can do rollback manually , here rollback will done automatically
                }
            }
        }
    }
}
