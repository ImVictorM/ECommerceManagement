using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponRestrictionAggregate.Entities;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.CouponRestrictionAggregate;

/// <summary>
/// Configures the <see cref="CategoryRestriction"/> restriction tables.
/// </summary>
public class CategoryRestrictionConfigurations : IEntityTypeConfiguration<CategoryRestriction>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CategoryRestriction> builder)
    {
        builder.ToTable("restriction_categories");

        ConfigureOwnedCategoriesAllowedTable(builder);
        ConfigureOwnedProductsNotAllowedTable(builder);
    }

    private static void ConfigureOwnedProductsNotAllowedTable(EntityTypeBuilder<CategoryRestriction> builder)
    {
        builder.OwnsMany(cr => cr.ProductsFromCategoryNotAllowed, productsBuilder =>
        {
            productsBuilder.ToTable("restriction_products_not_allowed_products");

            productsBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            productsBuilder.HasKey("id");

            productsBuilder.WithOwner().HasForeignKey("id_restriction_category");

            productsBuilder
                .Property(p => p.ProductId)
                .HasConversion(id => id.Value, value => ProductId.Create(value))
                .IsRequired();

            productsBuilder
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .IsRequired();
        });
    }

    private static void ConfigureOwnedCategoriesAllowedTable(EntityTypeBuilder<CategoryRestriction> builder)
    {
        builder.OwnsMany(cr => cr.CategoriesAllowed, categoriesBuilder =>
        {
            categoriesBuilder.ToTable("restriction_categories_allowed_categories");

            categoriesBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            categoriesBuilder.HasKey("id");

            categoriesBuilder.WithOwner().HasForeignKey("id_restriction_category");

            categoriesBuilder
                .Property(c => c.CategoryId)
                .HasConversion(id => id.Value, value => CategoryId.Create(value))
                .IsRequired();

            categoriesBuilder
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .IsRequired();
        });
    }
}
