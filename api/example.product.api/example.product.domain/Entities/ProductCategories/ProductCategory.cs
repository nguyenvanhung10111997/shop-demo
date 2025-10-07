using example.domain.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace example.product.domain.Entities;

[Table("ProductCategory", Schema = "PRO")]
public class ProductCategory : AuditEntity<int>
{
    public required string CategoryName { get; set; }

    public string? Description { get; set; }
}

public static class ProductCategoryModelBuilder
{
    public static void CreateProductCategoryBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.Property(p => p.CategoryName)
               .HasMaxLength(200)
               .IsUnicode(false);

            entity.Property(p => p.Description)
               .HasMaxLength(500)
               .IsUnicode(false);
        });
    }
}