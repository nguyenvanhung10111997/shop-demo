using example.domain.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace example.order.domain.Entities;

[Table("OrderDetail", Schema = "ORD")]
public class OrderDetail : AuditEntity<Guid>
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public required string ProductCode { get; set; }

    public required string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }
}

public static class OrderDetailModelBuilder
{
    public static void CreateOrderDetailBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.Property(p => p.ProductCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(p => p.ProductName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(p => p.Description)
               .HasMaxLength(500)
               .IsUnicode(false);
        });
    }
}