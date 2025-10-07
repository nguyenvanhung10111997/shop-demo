using example.domain.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace example.order.domain.Entities;

[Table("OrderStatus", Schema = "ORD")]
public class OrderStatus : AuditEntity<int>
{
    public required string StatusCode { get; set; }

    public required string StatusName { get; set; }

    public string? Description { get; set; }
}

public static class OrderStatusModelBuilder
{
    public static void CreateOrderStatusBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.Property(p => p.StatusCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(p => p.StatusName)
               .HasMaxLength(200)
               .IsUnicode(false);

            entity.Property(p => p.Description)
               .HasMaxLength(500)
               .IsUnicode(false);
        });
    }
}
