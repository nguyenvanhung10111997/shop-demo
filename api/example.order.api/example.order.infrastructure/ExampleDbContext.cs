using example.order.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace example.infrastructure
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
        { 
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<OrderTemp> OrderTemp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.CreateOrderBuilder();
            modelBuilder.CreateOrderDetailBuilder();
            modelBuilder.CreateOrderStatusBuilder();
            modelBuilder.CreateOrderTempBuilder();
        }

        /* Migration DB CLI
         * Step 1: add-migration InitialExampleDB
         * Step 2: dotnet ef migrations add InitialExampleDB
         * Step 3: update-database -verbose
         * Scaffold-DbContext "Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

         */
    }
}
