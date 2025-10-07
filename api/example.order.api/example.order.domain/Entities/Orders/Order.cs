using example.domain.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace example.order.domain.Entities;

[Table("Order", Schema = "ORD")]
public class Order : AuditEntity<Guid>
{
    public required string OrderCode { get; set; }

    public int OrderStatusId { get; set; }

    public int TotalQuantity { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Description { get; set; }
}

public static class OrderModelBuilder
{
    public static void CreateOrderBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.Property(p => p.OrderCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(p => p.Description)
               .HasMaxLength(500)
               .IsUnicode(false);

            // 🔹 Add index for OrderCode (unique)
            entity.HasIndex(p => new { p.OrderCode, p.IsDeleted })
                .IsUnique()
                .HasAnnotation("Relational:IndexName", "IX_Order_OrderCode");

            // 🔹 Add index for OrderStatusId
            entity.HasIndex(p => new { p.OrderStatusId, p.IsDeleted })
                .HasAnnotation("Relational:IndexName", "IX_Order_OrderStatusId");
        });
    }
}