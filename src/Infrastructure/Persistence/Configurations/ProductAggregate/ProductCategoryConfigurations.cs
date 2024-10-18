using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.ProductAggregate;

/// <summary>
/// Configures the <see cref="ProductCategory"/> entity to its table.
/// </summary>
public sealed class ProductCategoryConfigurations : IEntityTypeConfiguration<ProductCategory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("product_categories");

        builder.HasKey(pc => pc.Id)
            ;
        builder
            .Property(pc => pc.Id)
            .HasConversion(
                id => id.Value,
                value => ProductCategoryId.Create(value)
            )
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(productCategory => productCategory.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(pc => pc.Name).IsUnique();

        builder.HasData(ProductCategory.List());
    }
}
