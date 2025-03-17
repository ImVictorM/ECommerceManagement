using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Coupons;

internal sealed class CouponCategoryRestrictionConfigurations
    : EntityTypeConfigurationDependency<CouponCategoryRestriction>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<CouponCategoryRestriction> builder)
    {
        builder.ToTable("restriction_categories");

        ConfigureOwnedCategoriesAllowedTable(builder);
        ConfigureOwnedProductsNotAllowedTable(builder);
    }

    private static void ConfigureOwnedProductsNotAllowedTable(
        EntityTypeBuilder<CouponCategoryRestriction> builder
    )
    {
        builder.OwnsMany(
            cr => cr.ProductsFromCategoryNotAllowed,
            productsNotAllowedBuilder =>
            {
                productsNotAllowedBuilder
                    .UsePropertyAccessMode(PropertyAccessMode.Field);

                productsNotAllowedBuilder
                    .ToTable("restriction_products_not_allowed_products");

                productsNotAllowedBuilder
                    .Property<long>("id")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                productsNotAllowedBuilder.HasKey("id");

                productsNotAllowedBuilder
                    .WithOwner()
                    .HasForeignKey("id_restriction_category");

                productsNotAllowedBuilder
                    .Property(p => p.ProductId)
                    .HasConversion(id => id.Value, value => ProductId.Create(value))
                    .IsRequired();

                productsNotAllowedBuilder
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(p => p.ProductId)
                    .IsRequired();
            }
        );
    }

    private static void ConfigureOwnedCategoriesAllowedTable(
        EntityTypeBuilder<CouponCategoryRestriction> builder
    )
    {
        builder.OwnsMany(cr => cr.CategoriesAllowed, categoriesAllowedBuilder =>
        {
            categoriesAllowedBuilder
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            categoriesAllowedBuilder
                .ToTable("restriction_categories_allowed_categories");

            categoriesAllowedBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            categoriesAllowedBuilder.HasKey("id");

            categoriesAllowedBuilder
                .WithOwner()
                .HasForeignKey("id_restriction_category");

            categoriesAllowedBuilder
                .Property(c => c.CategoryId)
                .HasConversion(id => id.Value, value => CategoryId.Create(value))
                .IsRequired();

            categoriesAllowedBuilder
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .IsRequired();
        });
    }
}
