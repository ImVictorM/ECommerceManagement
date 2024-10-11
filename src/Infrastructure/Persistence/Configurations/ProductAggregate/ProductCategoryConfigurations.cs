using Domain.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.ProductAggregate;

/// <summary>
/// Configures the <see cref="ProductCategory"/> value object to its table.
/// </summary>
public sealed class ProductCategoryConfigurations : IEntityTypeConfiguration<ProductCategory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("product_categories");

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .Property(productCategory => productCategory.Name)
            .HasMaxLength(60)
            .IsRequired();
    }
}
