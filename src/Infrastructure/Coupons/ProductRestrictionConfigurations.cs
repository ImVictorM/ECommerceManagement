using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Coupons;

/// <summary>
/// Configures the <see cref="ProductRestriction"/> restriction tables.
/// </summary>
public class ProductRestrictionConfigurations : IEntityTypeConfiguration<ProductRestriction>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ProductRestriction> builder)
    {
        builder.ToTable("restriction_products");

        ConfigureOwnedProductsAllowedTable(builder);
    }

    private static void ConfigureOwnedProductsAllowedTable(EntityTypeBuilder<ProductRestriction> builder)
    {
        builder.OwnsMany(pr => pr.ProductsAllowed, productsBuilder =>
        {
            productsBuilder.ToTable("restriction_products_allowed_products");

            productsBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            productsBuilder.HasKey("id");

            productsBuilder.WithOwner().HasForeignKey("id_restriction_product");

            productsBuilder
                .Property(p => p.ProductId)
                .HasConversion(id => id.Value, value => ProductId.Create(value))
                .IsRequired();

            productsBuilder
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(p => p.ProductId);
        });
    }
}
