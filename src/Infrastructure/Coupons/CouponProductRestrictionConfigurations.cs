using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Coupons;

internal sealed class CouponProductRestrictionConfigurations
    : EntityTypeConfigurationDependency<CouponProductRestriction>
{
    public override void Configure(
        EntityTypeBuilder<CouponProductRestriction> builder
    )
    {
        builder.ToTable("restriction_products");

        ConfigureOwnedProductsAllowedTable(builder);
    }

    private static void ConfigureOwnedProductsAllowedTable(
        EntityTypeBuilder<CouponProductRestriction> builder
    )
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
