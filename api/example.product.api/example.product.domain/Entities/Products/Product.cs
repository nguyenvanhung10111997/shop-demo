using example.domain.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace example.product.domain.Entities;

[Table("Product", Schema = "PRO")]
public class Product : AuditEntity<Guid>
{
    public required string ProductCode { get; set; }

    public required string ProductName { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public float Rating { get; set; }

    public string ImageUrl { get; set; }

    public int Stock { get; set; }

    public string? Description { get; set; }
}

public static class ProductModelBuilder
{
    public static void CreateProductBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.Property(p => p.ProductCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(p => p.ProductName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(p => p.ImageUrl)
               .HasMaxLength(500)
               .IsUnicode(false);

            entity.Property(p => p.Description)
               .HasMaxLength(500)
               .IsUnicode(false);

            // 🔹 Add index for ProductCode (unique)
            entity.HasIndex(p => new { p.ProductCode, p.IsDeleted })
                .IsUnique()
                .HasAnnotation("Relational:IndexName", "IX_Product_ProductCode");

            // 🔹 Add index for CategoryId
            entity.HasIndex(p => new { p.CategoryId, p.IsDeleted })
                .HasAnnotation("Relational:IndexName", "IX_Product_CategoryId");
        });
    }
}