using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.ProductAggregate;

/// <summary>
/// Configure the tables related directly with the <see cref="Product"/> aggregate.
/// </summary>
public sealed class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ConfigureProductTable(builder);
        ConfigureOwnedInventoryTable(builder);
        ConfigureOwnedProductDiscountTable(builder);
        ConfigureOwnedProductImageTable(builder);
    }

    /// <summary>
    /// Configures the products table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public static void ConfigureProductTable(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(product => product.Id);

        builder
            .Property(product => product.Id)
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne(p => p.ProductCategory)
            .WithMany()
            .HasForeignKey("id_category")
            .IsRequired();

        builder
            .Property(product => product.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(product => product.Description)
            .HasMaxLength(255);

        builder
           .Property(product => product.Price)
           .IsRequired();

        builder
           .Property(product => product.IsActive)
           .IsRequired();
    }

    /// <summary>
    /// Configures the inventories table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedInventoryTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsOne(
            product => product.Inventory,
            inventoryBuilder =>
            {
                inventoryBuilder.ToTable("inventories");

                inventoryBuilder.HasKey(inventory => inventory.Id);

                inventoryBuilder
                    .Property(inventory => inventory.Id)
                    .HasConversion(
                        id => id.Value,
                        value => InventoryId.Create(value)
                    )
                    .IsRequired();

                inventoryBuilder.WithOwner().HasForeignKey("id_product");

                inventoryBuilder
                    .Property("id_product")
                    .IsRequired();

                inventoryBuilder
                    .Property(inventory => inventory.QuantityAvailable)
                    .IsRequired();
            });
    }

    /// <summary>
    /// Configures the product_discounts table.
    /// </summary>
    /// <param name="builder">Then entity type builder.</param>
    private static void ConfigureOwnedProductDiscountTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(
            product => product.ProductDiscounts,
            productDiscountBuilder =>
            {
                productDiscountBuilder.ToTable("product_discounts");

                productDiscountBuilder.HasKey(productDiscount => productDiscount.Id);

                productDiscountBuilder
                    .Property(productDiscount => productDiscount.Id)
                    .HasConversion(
                        id => id.Value,
                        value => ProductDiscountId.Create(value)
                    )
                    .IsRequired();

                productDiscountBuilder.WithOwner().HasForeignKey("id_product");

                productDiscountBuilder
                    .Property("id_product")
                    .IsRequired();

                productDiscountBuilder.OwnsOne(pd => pd.Discount, DiscountNavigationBuilderConfigurations.Configure);
            });
    }

    /// <summary>
    /// Configures the product image table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedProductImageTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(
            product => product.ProductImages,
            productImageBuilder =>
            {
                productImageBuilder.ToTable("product_images");

                productImageBuilder.HasKey(productImage => productImage.Id);

                productImageBuilder
                    .Property(productImage => productImage.Id)
                    .HasConversion(
                        id => id.Value,
                        value => ProductImageId.Create(value)
                    )
                    .IsRequired();

                productImageBuilder.WithOwner().HasForeignKey("id_product");

                productImageBuilder
                    .Property("id_product")
                    .IsRequired();

                productImageBuilder
                    .Property(productImage => productImage.Url)
                    .IsRequired();
            });
    }
}
