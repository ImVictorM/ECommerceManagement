using Domain.ProductCategoryAggregate;
using Domain.ProductCategoryAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="ProductCategory"/> aggregate to entity framework.
/// </summary>
public sealed class ProductCategoryConfigurations : IEntityTypeConfiguration<ProductCategory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        ConfigureProductCategoryTable(builder);
    }

    /// <summary>
    /// Configures the product category table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureProductCategoryTable(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("product_categories");

        builder.HasKey(productCategory => productCategory.Id);

        builder
            .Property(productCategory => productCategory.Id)
            .HasConversion(
                id => id.Value,
                value => ProductCategoryId.Create(value)
            )
            .IsRequired();

        builder
            .Property(productCategory => productCategory.Name)
            .HasMaxLength(60)
            .IsRequired();
    }
}
