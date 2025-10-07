using example.domain.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace example.order.domain.Entities;

[Table("OrderTemp", Schema = "ORD")]
public class OrderTemp : AuditEntity<Guid>
{
    public required string OrderCode { get; set; }

    public string? Description { get; set; }

    public string? TempData { get; set; }
}

public static class OrderTempModelBuilder
{
    public static void CreateOrderTempBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderTemp>(entity =>
        {
            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.Property(p => p.OrderCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(p => p.Description)
               .HasMaxLength(500)
               .IsUnicode(false);

            entity.Property(p => p.TempData)
              .HasMaxLength(int.MaxValue);

            // 🔹 Add index for OrderCode (unique)
            entity.HasIndex(p => new { p.OrderCode, p.IsDeleted })
                .IsUnique()
                .HasAnnotation("Relational:IndexName", "IX_Order_OrderCode");
        });
    }
}