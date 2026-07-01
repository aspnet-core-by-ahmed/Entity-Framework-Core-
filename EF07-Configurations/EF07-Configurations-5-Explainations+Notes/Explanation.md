# EF Core – Model Configuration Notes (Combined)

This file combines two source documents, preserved in full without any deletions:

1. `All.cs` — EF Core Model Configuration (code + comments)
2. EF Core – Full Concept Overview (markdown notes)

---

## Part 1 — EF Core Model Configuration (Full Organized Explanation)

> Original source: C# file (`All.cs`) — reproduced in full inside a code block below.

```csharp
// =============================================================
// EF Core - Model Configuration (Full Organized Explanation)
// This file contains a clean, structured, corrected, and fully
// organized explanation for all concepts you wrote previously.
// =============================================================

using EF007.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF007.Explaination
{
    internal class All
    {
        // =============================================================
        // EF Core Model Overview
        // Model = [Entities (classes) + DbContext (session with database)]
        // EF Core acts as an ORM (Object Relational Mapper)
        // =============================================================

        // EF Core supports 3 development approaches:
        // 1) Database First  => [DB -> Model]
        // 2) Model First     => [DB <- Model]
        // 3) Code First      => [DB <- Model] (using migrations to generate DB)


        // =============================================================
        // Example Application (Twitter Simulation)
        // =============================================================
        // Example Tweets:
        //  User #1 (Jan 11 2022 19:35) => "EF-Core is an open-source ORM"
        //  User #2 (Jan 11 2022 20:00) => "Yes, and also lightweight"
        //  User #3 (Jan 12 2022 18:00) => "Yes, and cross-platform"

        // Database will include:
        //  - Users table
        //  - Tweets table
        //  - Comments table

        // Model (in code) will include:
        //  - User class
        //  - Tweet class
        //  - Comment class
        //  - AppDbContext : DbContext (bridge/session with database)
        //      inside it: DbSet<Tweet> Tweets
        //                 DbSet<User> Users
        //                 DbSet<Comment> Comments


        // =============================================================
        // EF Core Configuration System
        // Convention Over Configuration
        // =============================================================
        // EF works first by Convention (automatic mapping)
        // If conventions fail → you override using Configuration.

        // Convention Requirements:
        // 1) DbSet property name should match table name.
        // 2) Class property names must match column names.
        // 3) Class name does NOT matter.
        // 4) Primary Key conventions:
        //      Id / id / ID
        //      ClassNameId / classNameId / ClassNameID

        // If a property name doesn’t match a column → EF breaks → You must configure it.


        // =============================================================
        // 1) Data Annotations (Override Convention)
        // =============================================================
        // Examples:
        //  [Table("tblComments")]  → Maps class to table tblComments
        //  [Column("UserId")]      → Maps property to specific column
        //
        // Used ONLY when Convention is broken.


        // =============================================================
        // 2) Fluent API (Override Using OnModelCreating)
        // =============================================================
        // More powerful than Data Annotations.
        // Keeps the Entities clean.
        // All configuration in one centralized place.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table Mapping
            modelBuilder.Entity<User>().ToTable("tblUsers");
            modelBuilder.Entity<Tweet>().ToTable("tblTweets");
            modelBuilder.Entity<Comment>().ToTable("tblComments");

            // Column Mapping Example
            modelBuilder
                .Entity<User>()
                .Property(p => p.Id)
                .HasColumnName("CommentId");
        }


        // =============================================================
        // 3) Grouping Configuration (Best Practice)
        // =============================================================
        // Using IEntityTypeConfiguration<T>
        // Better separation of concerns, cleaner architecture.
        // One config class per entity.


        // This is the BEST way (Grouping configuration)
        // Better separation of concerns
        // Create folder Config inside folder Data
        // Put UserConfiguration inside it
        // Make it public
        // Inherit from IEntityTypeConfiguration<User>
        // Implement Configure()
        // Add logic inside

        // note: create config class for each entity



        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable("tblUsers");
            }
        }

        public class TweetConfiguration : IEntityTypeConfiguration<Tweet>
        {
            public void Configure(EntityTypeBuilder<Tweet> builder)
            {
                builder.ToTable("tblTweets");
            }
        }

        public class CommentConfiguration : IEntityTypeConfiguration<Comment>
        {
            public void Configure(EntityTypeBuilder<Comment> builder)
            {
                builder.ToTable("tblComments");
            }
        }


        // Apply configurations inside OnModelCreating
        protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Individual Calls (Not bad but slow , take space.....)
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new CommentConfiguration().Configure(modelBuilder.Entity<Comment>());
            new TweetConfiguration().Configure(modelBuilder.Entity<Tweet>());

            // Grouping through Assembly (Best Way)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }


        // =============================================================
        // Additional Explanation (as you wrote it)
        // =============================================================

        // by default EF-Configuration Use Convention
        // if the last conditions false you will use configuration inside model
        // by default EF-Configuration Use Convention
        // "Convention over configuration" paradigm
        // تقليل عدد التدخلات التي يتعين على المطور عملها في حال اتباع الاتفاقية
        // دون إلغاء إمكانية التخصيص مما يعطي مرونة في التعامل مع الفريمورك.

        // You will have 2 databases on SQL Server:
        // 1- FakeTwitterV1  => tables: tblComments, tblTweets, tblUsers
        // 2- FakeTwitterV2  => same structure but different column names

        // We will use FakeTwitterV1 to explain convention.
        // create appsettings and put this inside:

        /*
        {
           "constr": "Server = . ; Database = FakeTwitterV2 ; Integrated Security = SSPI ; TrustServerCertificate = True;"
        }

        // see diagram in SQL Server
        */

        // [Table("tblComments")] example:
        // If your DbSet is named Comments and the DB table is tblComments,
        // EF will throw "invalid object name Users".
        // So you must add [Table("tblComments")] to fix the mapping.

        // Example PK Convention:
        // If inside class User you rename UserId → Id
        // This respects PK convention
        // BUT breaks property-to-column naming.
        // So you must use:
        // [Column("UserId")]

        // OnModelCreating is virtual
        // you can override it to add additional logic.

}
```

---

## Part 2 — EF Core Full Concept Overview

> Original source: markdown notes — reproduced in full below.

# ===============================================================

# **EF Core - Full Concept Overview**

# ===============================================================

### 1️- Mapping & Conventions

- EF Core **does not look at the database structure first but it look to ur cood**.

- EF Core **maps your code (Entities) to the database** based on **DbSet properties** and **class properties**.

- **Rules for automatic mapping (Conventions):**
  1. `DbSet` property names → **match table names** in database.
  2. Entity **property names** → **match column names** in database.
  3. **Case-sensitive** → `Wallets` ≠ `wallets`.
  4. Primary Key detection:
     - Property names: `Id`, `id`, `ID`
     - Or `<ClassName>Id`, `<ClassName>ID`, `<className>Id`
  5. If property names break convention → **EF will not detect Primary Key** → must configure manually.

- **Important:** EF Core determines **Primary Key from the code**, not from the database.
  - Example:

    public int Id { get; set; } // EF recognizes as PK
    public int UserId { get; set; } // EF recognizes as PK if class is User
    public int Number { get; set; } // ❌ EF will NOT recognize as PK → need config

============================================================================================

### 2️- How to fix Primary Key if convention fails

1. **Data Annotation**

[Key]
public int Number { get; set; }

2. **Fluent API**

builder.HasKey(u => u.Number);

============================================================================================

### 3️- EF Core Approaches / Development Strategies

#### **A) Database First**

- You have an **existing database**.
- EF Core generates **Entities & DbContext** from DB.
- Old-fashioned: manually writing entities from DB → not recommended.
- Recommended: use **`Scaffold-DbContext`** command:

==> dotnet ef dbcontext scaffold "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer -o Entities -c AppDbContext

#### **B) Code First (Modern / Recommended)**

- You **write the code/entities first**.
- EF Core **creates the database** from your code.
- Use **Migrations** to generate DB:

==> dotnet ef migrations add InitialCreate
==> dotnet ef database update

#### **C) Model / Design First (Almost extinct)**

- You **design the model visually** first.
- Then generate **Database** → then generate **code**.
- Rarely used today.

============================================================================================

### 4️- DbSet & Property Naming

- **DbSet<T>** → must match table name.

public DbSet<User> Users { get; set; } // maps to "Users" table

- **Entity property names** → must match column names (unless configured)

public int Id { get; set; } // maps to "Id" column
public string Name { get; set; } // maps to "Name" column

- **Case-sensitive:**

DbSet<Wallet> Wallets; // maps to "Wallets" table
DbSet<Wallet> wallets; // ❌ maps to "wallets", may cause errors

============================================================================================

### 5️- Configuration (When Convention Fails)

- **Data Annotations**

[Table("tblUsers")]
[Column("UserId")]
[Key]
public int Number { get; set; }

- **Fluent API**

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>().ToTable("tblUsers");
    modelBuilder.Entity<User>().HasKey(u => u.Number);
    modelBuilder.Entity<User>().Property(u => u.Number).HasColumnName("UserId");

}

============================================================================================

### 6️- Notes on Best Practices

- **Always prefer convention** → less manual work.

- **Convention over configuration** paradigm:
  - Reduces errors.
  - Makes code cleaner.
  - You can still override for flexibility.

- **Golden Rule:**
  > One DbContext = One unit of work. Never share it across threads (for concurrency safety).

============================================================================================

### 7️- Summary Table for EF Core Convention

| Concept                      | Convention Example                  | Requires Config if Breaks? |
| ---------------------------- | ----------------------------------- | -------------------------- |
| DbSet → Table Name           | `DbSet<Wallet> Wallets` → "Wallets" | ✅                         |
| Class Property → Column Name | `public int Id {get;set;}` → "Id"   | ✅                         |
| Primary Key                  | `Id` / `UserId` / `ClassNameId`     | ✅                         |
| Non-convention PK            | `Number` / `Code` / `UserNumber`    | ❌ Need config             |

============================================================================================

### 8️- Recommended Workflow Today

1. Use **Code First** → write entities + DbContext.
2. Follow **naming conventions** for easy automatic mapping.
3. Use **Scaffold** only for legacy DB.
4. Override with **Data Annotations** or **Fluent API** if conventions fail.
