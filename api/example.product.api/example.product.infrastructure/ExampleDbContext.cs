using example.product.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace example.infrastructure
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
        { 
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.CreateProductBuilder();
            modelBuilder.CreateProductCategoryBuilder();
        }

        /* Migration DB CLI
         * Step 1: add-migration InitialExampleDB
         * Step 2: dotnet ef migrations add InitialExampleDB
         * Step 3: update-database -verbose
         * Scaffold-DbContext "Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

         */
    }
}
