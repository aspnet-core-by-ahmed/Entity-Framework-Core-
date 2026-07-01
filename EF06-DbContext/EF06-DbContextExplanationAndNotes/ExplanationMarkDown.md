# DbContext — Complete Guide

## What is DbContext?

`DbContext` = create a "session" with the database. This session lets you store, retrieve, and update data.

`DbContext` is a **mix of two patterns**:

1. **Unit Of Work Pattern**
   - Tracks ALL changes (Add / Update / Delete) during one operation.
   - Does NOT write to the database immediately.
   - Writes to the database only when `SaveChanges()` is called.

2. **Repository Pattern**
   - A class that contains the logic to deal with the database.
   - `DbSet<T>` works as a repository for each entity.

```csharp
public DbSet<Wallet> Wallets { get; set; } = null!;
// This represents a TABLE in the database.
```

---

## DbContext Configuration — What does it mean?

"Configuring the DbContext" simply means defining how the DbContext will connect to the database and how it should behave internally.

Configuration typically includes:

- Selecting the database provider (e.g., SQL Server, SQLite)
- Setting the connection string
- Controlling tracking behavior, logging, and caching
- Applying model configuration rules (entities, keys, relationships)

In EF Core, DbContext can be configured in two ways:

1. **Externally** → using `AddDbContext()` inside `Program.cs` (Dependency Injection)
2. **Internally** → using `OnConfiguring()` inside the DbContext class itself

Both approaches achieve the same goal: telling EF Core how the context should operate. The external approach is preferred in real applications for better structure, flexibility, and maintainability.

> "Configuration" بالعربي معناها: الإعدادات / تهيئة النظام / ضبط الإعدادات — حسب السياق، معناها ضبط الطريقة اللي النظام أو الأداة هتشتغل بيها.

---

## 1) Configure DbContext INTERNALLY using `OnConfiguring`

- `DbContext` has a virtual method called `OnConfiguring()`.
- "virtual" means original logic exists AND you can add extra logic.
- You override it to provide database configuration.
- The configuration is inside the context class itself.
- You use this way mainly in **Console Apps** or simple apps.

```csharp
public class AppDbContext : DbContext
{
    // This property creates a table named "Wallets"
    // DbSet = a table in the database
    // EF Core will use the Wallet class to generate columns inside this table
    public DbSet<Wallet> Wallets { get; set; } = null!;

    // This method configures how the DbContext connects to the database
    // EF Core calls this method automatically when the context is created
    // It is used when the context does NOT receive options from the outside (no DI)
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Call the base class (keeps EF Core default behavior)
        // Not required in most cases, but safe to leave
        // You can also place this at the END — the order does not matter
        base.OnConfiguring(optionsBuilder);

        // Create configuration builder to read appsettings.json
        // This is needed because in this pattern the context reads settings by itself
        // (not receiving them from dependency injection)
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json") // load the JSON file
            .Build(); // build the configuration object

        // Read the connection string from the JSON file
        // "constr" is the key that holds the connection string value
        var constr = configuration.GetSection("constr").Value;

        // Tell EF Core to use SQL Server as the Provider
        // and apply the connection string
        // This is where the database type and connection details are chosen
        optionsBuilder.UseSqlServer(constr);
    }
}
```

### Why is there no "options" here, only "optionsBuilder"?

In `OnConfiguring`, EF Core automatically provides a `DbContextOptionsBuilder` object (`optionsBuilder`). You don't need to pass or receive a separate "options" object here.

- `optionsBuilder` = the tool you use to set up the DbContext (provider, connection, etc.).
- In external setups (like ASP.NET Core with DI), EF Core gives you a **fully built** `DbContextOptions` object from outside.
- Inside `OnConfiguring`, you are responsible for **building** the options yourself using `optionsBuilder`. That's why you only see `optionsBuilder`, not `options`.

**Summary:**
| Term | Meaning |
|---|---|
| `optionsBuilder` | The tool you use to set up the DbContext (provider, connection, etc.) |
| `options` | Fully configured options (used when injected from outside) |

### Calling the context (parameterless constructor)

```csharp
using (var context = new AppDbContext())
{
    // db logic here
}
```

---

## 2) Configure DbContext EXTERNALLY (`DbContextOptions`)

`DbContext` has 2 constructors:

1. Parameterless
2. Constructor with `DbContextOptions`

In this way, we pass `DbContextOptions` from OUTSIDE. This is the standard way in ASP.NET Core.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Wallet> Wallets { get; set; } = null!;

    public AppDbContext(DbContextOptions options) : base(options)
    {
        // Empty – EF handles everything
    }
}
```

```csharp
// IN Program.cs
class Program
{
    public static void Main()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var constr = config.GetSection("constr").Value;

        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(constr);

        var options = optionsBuilder.Options;

        using (var context = new AppDbContext(options))
        {
            // logic
        }
    }
}
```

---

## 3) DbContext WITH Dependency Injection

- `IServiceCollection` = a list where you register services.
- `AddDbContext()` = register DbContext inside DI.
- `IServiceProvider` = creates objects for you when needed.
- This way removes coupling because you don't `new` the DbContext yourself.
- ASP.NET Core uses this method always.

```csharp
public class AppDbContext : DbContext
{
    // This creates a table named "Wallets" in the database
    // DbSet<Wallet> represents the table structure in EF Core
    public DbSet<Wallet> Wallets { get; set; } = null!;

    // Constructor receives "options" from outside (from DI)
    // - options = fully configured DbContext options (provider, connection string, etc.)
    // - We just pass it to the base DbContext constructor
    // - This way, OnConfiguring is not needed when using DI
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}
```

```csharp
class Program
{
    public static void Main()
    {
        // 1- Read appsettings.json
        // - This contains our connection string "constr"
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var constr = config.GetSection("constr").Value;

        // 2- Create a collection of services
        // - Services = objects that the app can provide automatically to other parts of the code
        var services = new ServiceCollection();

        // 3- Register AppDbContext inside the DI container
        // - AddDbContext tells DI how to create AppDbContext
        // - The "options => options.UseSqlServer(constr)" lambda configures the database provider and connection string
        // - DI will now be able to give you a ready-to-use AppDbContext anywhere
        services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(constr)
        );

        // 4- Build the DI container
        // - This creates the provider that knows how to supply all the registered services
        IServiceProvider provider = services.BuildServiceProvider();

        // 5- Request AppDbContext from DI
        // - Instead of manually creating "new AppDbContext()", we ask the provider
        // - DI gives a fully configured context with the connection string and SQL Server provider
        using (var context = provider.GetService<AppDbContext>())
        {
            // Now we can use "context" to access the database
            // Example: context.Wallets.Add(new Wallet { ... });
        }
    }
}
```

### Summary

- `optionsBuilder`: used when configuring DbContext manually inside `OnConfiguring()`
- `options`: fully built settings provided from outside, typically by DI
- `services`: a container that holds all objects your app can provide automatically
- DI (Dependency Injection): a system that gives objects like `AppDbContext` automatically, without you calling `new` yourself

### Why this way?

- Because DI handles lifetime, creation, disposal.
- It allows many classes to depend on DbContext without manually `new`-ing it.
- Used in ASP.NET Core controllers, services, repositories.

### Note: `GetService` vs `GetRequiredService`

```csharp
using (var context = provider.GetService<AppDbContext>())
{
    // GetService => tries to get the service
    // Returns null if the service is NOT registered
}

using (var context = provider.GetRequiredService<AppDbContext>())
{
    // GetRequiredService => tries to get the service
    // Throws an exception if the service is NOT registered
}
```

---

## 4) DbContext FACTORY (`IDbContextFactory`)

Factory Pattern = shift object creation to a factory class.

### Why do we need DbContextFactory?

- Some apps (like Blazor) use DI BUT do NOT create a scope that matches DbContext lifetime.
- Some apps need MULTIPLE DbContext instances inside the SAME scope.
- DbContextFactory solves this problem by allowing you to create DbContext manually.

### Requirements

1. Public constructor that accepts `DbContextOptions`
2. You must DISPOSE the context manually using `using()`

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Wallet> Wallets { get; set; } = null!;

    public AppDbContext(DbContextOptions options) : base(options) { }
}

class Program
{
    public static void Main()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var connectionString = config.GetSection("constr").Value;

        var services = new ServiceCollection();

        // Register factory instead of normal DbContext
        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        IServiceProvider provider = services.BuildServiceProvider();

        var factory = provider.GetService<IDbContextFactory<AppDbContext>>();

        using (var context = factory!.CreateDbContext())
        {
            // logic
        }
    }
}
```

### Why this way?

- Blazor uses this.
- Background jobs need multiple DbContext in the same scope.
- You need to create many DbContext instances manually.

---

## DbContext Lifetime — Tracking

Lifetime starts when you create the object using `new AppDbContext(options)`. During this lifetime, DbContext tracks ALL changes to DbSet objects.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Wallet> Wallets { get; set; } = null!;

    public AppDbContext(DbContextOptions options) : base(options) { }
}

class Program
{
    public static void Main()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var constr = config.GetSection("constr").Value;

        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(constr);

        using (var context = new AppDbContext(optionsBuilder.Options))
        {
            var w1 = new Wallet { Holder = "Ahmed", Balance = 1000m };
            context.Wallets.Add(w1);  // NOW Tracking starts

            var w2 = new Wallet { Holder = "Maria", Balance = 5000m };
            context.Wallets.Add(w2);

            // NOTHING is saved yet.

            context.SaveChanges();   // Unit Of Work writes ALL changes at once.
        } // Dispose happens here
    }
}
```

---

## DbContext Logging

Logging helps you see what SQL queries EF Core runs behind the scenes. You can set it up in the DbContext options to print logs to the console.

- Useful for debugging and understanding database interactions.
- Use `LogTo()` method to send logs to a place like `Console.WriteLine`.
- You can choose the log level, like `Information`, to control what gets logged.

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string constr = "YourConnectionStringHere"; // Replace with your actual connection string.
        optionsBuilder
            .UseSqlServer(constr)
            .LogTo(Console.WriteLine, LogLevel.Information); // Logs SQL commands to console at Information level.
    }
}
```

```csharp
// Example usage:
// In your main program, when you use the context, it will log the SQL.
static void Main(string[] args)
{
    using var context = new AppDbContext();
    var wallet = context.Wallets.FirstOrDefault(); // This will log the SELECT SQL query to console.
}

// Output in console might look like:
// Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
// SELECT TOP(1) [w].[Id], [w].[Balance], [w].[Holder]
// FROM [Wallets] AS [w]
```

---

## DbContext & Concurrency

### 1) Concurrency Problem (Why it happens?)

**Problem:**

- DbContext is **NOT thread-safe**.
- You cannot use the same DbContext instance in multiple threads.
- If two threads use the same DbContext at the same time, EF Core throws an error:

  > "A second operation started on this context before a previous operation completed."

**Why?**

- DbContext keeps an internal change-tracking state.
- When two threads try to modify this state at the same time, the context becomes corrupted and EF Core stops the program.

The example below shows using ONE shared DbContext in two threads. This will cause concurrency errors.

```csharp
using EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EF.Entities;

namespace TryCodes
{
    class Program
    {
        public static AppDbContext sharedContext = CreateDbContext();

        public static void Main()
        {
            var task1 = Task.Run(() => Job1());
            var task2 = Task.Run(() => Job2());

            using (var context = CreateDbContext())
            {
                foreach (var wallet in context.Wallets)
                {
                    Console.WriteLine(wallet);
                }
            }
        }

        static void Job1()
        {
            // Two threads writing on SAME DbContext → unsafe
            sharedContext.Wallets.Add(new Wallet { Holder = "Ahmed", Balance = 1000m });
            sharedContext.SaveChanges();   // Can conflict
        }

        static void Job2()
        {
            sharedContext.Wallets.Add(new Wallet { Holder = "Sara", Balance = 2000m });
            sharedContext.SaveChanges();   // May throw exception
        }

        public static AppDbContext CreateDbContext()
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("C:\\Users\\hp\\Desktop\\TryCodesEF\\TryCodes\\appsettings.json")
                            .Build();

            var constr = config.GetSection("constr").Value;

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(constr);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
```

### 2) Solution #1 — Create DbContext inside each method

Best practice:

- DO NOT share DbContext.
- Create a fresh DbContext for every job / thread.
- Each job uses its own instance → NO conflicts.

This is the recommended way in multi-thread apps (console, background jobs, etc.)

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

static void Job1()
{
    using var context = CreateNewContext();  // New context for Job1
    context.Wallets.Add(new Wallet { Holder = "Ahmed", Balance = 1000m });
    context.SaveChanges();                   // Safe
}

static void Job2()
{
    using var context = CreateNewContext();  // New context for Job2
    context.Wallets.Add(new Wallet { Holder = "Sara", Balance = 2000m });
    context.SaveChanges();                   // Safe
}

static AppDbContext CreateNewContext()
{
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    var constr = config.GetValue<string>("constr");

    var services = new ServiceCollection();
    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(constr));

    var provider = services.BuildServiceProvider();
    return provider.GetRequiredService<AppDbContext>();
}

static void Main(string[] args)
{
    var task1 = Task.Run(() => Job1());
    var task2 = Task.Run(() => Job2());

    Task.WaitAll(task1, task2);
    Console.WriteLine("Done without errors!");
}
```

### 3) Solution #2 — Use Async Methods

**Important:**

- Using async _does not fix_ concurrency problems if you STILL share DbContext.
- Async only helps when operations run sequentially on the SAME thread.
- For real multi-thread situations, you MUST use separate DbContext instances.

The example below uses async but still shares one DbContext. This can still cause problems if true multi-threading happens.

```csharp
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

static AppDbContext context = new AppDbContext();  // Shared instance → still risky

static async Task Job1()
{
    context.Wallets.Add(new Wallet { Holder = "Ahmed", Balance = 1000m });
    await context.SaveChangesAsync();   // Async save
}

static async Task Job2()
{
    context.Wallets.Add(new Wallet { Holder = "Sara", Balance = 2000m });
    await context.SaveChangesAsync();   // Async save
}

static async Task Main(string[] args)
{
    var t1 = Job1();
    var t2 = Job2();

    await Task.WhenAll(t1, t2);

    Console.WriteLine("Done!");
}
```

### 4) Solution #3 — Using DbContextFactory (Best for Multi-Thread)

**Why DbContextFactory?**

- It allows generating a NEW DbContext instance every time you call `CreateDbContext()`.
- Each thread gets its OWN DbContext → NO SHARING.
- This completely removes DbContext concurrency errors.
- Factory is lightweight and optimized for threaded/background operations.

**When to use it?**

- Console apps
- Background jobs (Hosted Services, Workers)
- Multi-thread operations
- Long-running operations

> **Golden Point:** "DbContextFactory = Safest way to use EF Core in multi-thread apps."

```csharp
using EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EF.Entities;

namespace TryCodes
{
    internal class Program
    {
        // Global Factory (Thread-Safe by design)
        public static IDbContextFactory<AppDbContext>? factory = DbContextConfig();

        static void Main(string[] args)
        {
            // Running both jobs in parallel threads
            var task1 = Task.Run(() => Job1());
            var task2 = Task.Run(() => Job2());

            Task.WaitAll(task1, task2);

            // Reading data using a fresh context from factory
            using (var context = factory!.CreateDbContext())
            {
                foreach (var item in context.Wallets)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Done without concurrency errors!");
        }

        /******************************************************
         * Create Factory (DI Container + AddDbContextFactory)
         ******************************************************/
        public static IDbContextFactory<AppDbContext>? DbContextConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("C:\\Users\\hp\\Desktop\\TryCodesEF\\TryCodes\\appsettings.json")
                .Build();

            var constr = config.GetSection("constr").Value;

            var services = new ServiceCollection();

            // AddDbContextFactory → best choice for multi-thread usage
            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlServer(constr));

            IServiceProvider provider = services.BuildServiceProvider();

            return provider.GetService<IDbContextFactory<AppDbContext>>();
        }

        /******************************************************
         * Each job creates its OWN DbContext instance
         ******************************************************/
        static void Job1()
        {
            // NEW context instance (safe)
            using var context = factory!.CreateDbContext();

            context.Wallets.Add(new Wallet
            {
                Holder = "Rwan Ayman",
                Balance = 1500.00m
            });

            context.SaveChanges();
        }

        static void Job2()
        {
            using var context = factory!.CreateDbContext();

            context.Wallets.Add(new Wallet
            {
                Holder = "Osama Ayman",
                Balance = 1500.00m
            });

            context.SaveChanges();
        }
    }
}
```

### Summary of Solution #3 (DbContextFactory)

- Best way for parallel threads
- Each thread gets unique DbContext
- Completely prevents concurrency errors
- Recommended for Console apps + Background workers
- Fast & lightweight
- Factory itself is thread-safe

> **Remember:** "DbContextFactory gives you guaranteed isolated DbContext per thread, per task, per operation."

---

## DbContext Pooling

- DbContext is lightweight BUT has heavy internal services.
- Creating/destroying many DbContexts reduces performance.
- Pooling = instead of destroying the DbContext, it resets it and stores it in a pool.
- Next time you need a context, EF gives you one from the pool.
- Max pool size = 1024 contexts (default).

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Wallet> Wallets { get; set; } = null!;
    public AppDbContext(DbContextOptions options) : base(options) { }
}

class Program
{
    static void Main()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var constr = config.GetSection("constr").Value;

        var services = new ServiceCollection();

        // ENABLE Context Pooling
        services.AddDbContextPool<AppDbContext>(options =>
            options.UseSqlServer(constr)
        );

        var provider = services.BuildServiceProvider();

        using (var context = provider.GetService<AppDbContext>())
        {
            // logic
        }
    }
}
```

> Pooling increases performance dramatically in:
>
> - High traffic web apps
> - Multi-tenancy systems
> - Background workers

---

## Context Pooling — For Reading

### Context Pooling Problem

Creating a new DbContext every time (like in Solution 1 above) can be slow and use a lot of resources. Each new context needs to set up connections, which takes time in high-traffic apps like web servers. Without pooling, performance drops because of repeated creation and disposal of contexts.

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public class WalletService
{
    public void AddWallet(Wallet wallet)
    {
        using var context = new AppDbContext(); // New context each time - slow for many calls!
        context.Wallets.Add(wallet);
        context.SaveChanges();
    }
}

static void Main(string[] args)
{
    var service = new WalletService();
    // Simulate many operations - each creates a new context, which is inefficient.
    for (int i = 0; i < 1000; i++)
    {
        service.AddWallet(new Wallet { Holder = $"User{i}", Balance = 100m });
    }
    // This loop might take longer due to repeated context creation.
}
```

### Solution 1 — Enable Context Pooling in DI

Use DbContext pooling in dependency injection. EF Core reuses contexts from a pool. This recycles contexts instead of creating new ones, improving speed. Set it up with `AddDbContextPool()` instead of `AddDbContext()`.

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public class WalletService
{
    private readonly IServiceProvider _provider;

    public WalletService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void AddWallet(Wallet wallet)
    {
        using var context = _provider.GetRequiredService<AppDbContext>(); // Gets from pool - fast!
        context.Wallets.Add(wallet);
        context.SaveChanges();
    }
}

static void Main(string[] args)
{
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    var constr = config.GetValue<string>("constr");

    var services = new ServiceCollection();
    services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(constr)); // Enable pooling!
    services.AddSingleton<WalletService>();

    var provider = services.BuildServiceProvider();
    var walletService = provider.GetRequiredService<WalletService>();

    // Simulate many operations - faster with pooling.
    for (int i = 0; i < 1000; i++)
    {
        walletService.AddWallet(new Wallet { Holder = $"User{i}", Balance = 100m });
    }
    Console.WriteLine("Done faster with pooling!");
}
```

### Solution 2 — Configure Pool Size

When using pooling, you can set the max pool size to control how many contexts are reused. Default is 128, but you can change it based on your app's needs. This helps in very high-load scenarios to avoid running out of connections.

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

static void Main(string[] args)
{
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    var constr = config.GetValue<string>("constr");

    var services = new ServiceCollection();
    services.AddDbContextPool<AppDbContext>(options =>
    {
        options.UseSqlServer(constr);
    }, poolSize: 256); // Set custom pool size, e.g., 256.

    var provider = services.BuildServiceProvider();
    using var context = provider.GetRequiredService<AppDbContext>();
    // Use the context - it comes from the pool with custom size.
    Console.WriteLine("Pooling with custom size enabled!");
}
```

---

## Quick Reference Table

| Method                                    | Best Used In                          | Key Notes                                                                   |
| ----------------------------------------- | ------------------------------------- | --------------------------------------------------------------------------- |
| **Internal (`OnConfiguring`)**            | Console apps, simple apps             | Context reads its own config; no DI needed                                  |
| **External (`DbContextOptions`)**         | Manual setup outside the class        | Options built and passed in from outside                                    |
| **Dependency Injection (`AddDbContext`)** | ASP.NET Core apps                     | Scoped lifetime, auto dispose, testable                                     |
| **Factory (`AddDbContextFactory`)**       | Blazor, background jobs, multi-thread | You manually create & dispose each context                                  |
| **Pooling (`AddDbContextPool`)**          | High-traffic apps                     | Reuses contexts instead of destroying them; default pool size 128, max 1024 |
