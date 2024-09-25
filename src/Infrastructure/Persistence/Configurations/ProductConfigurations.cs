using Domain.DiscountAggregate;
using Domain.DiscountAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.ProductCategoryAggregate;
using Domain.ProductCategoryAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="Product"/> aggregate to entity framework.
/// </summary>
public sealed class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ConfigureProductTable(builder);
        ConfigureInventoryTable(builder);
        ConfigureProductDiscountTable(builder);
        ConfigureProductImageTable(builder);
    }

    /// <summary>
    /// Configures the product table.
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
            .HasOne<ProductCategory>()
            .WithMany()
            .HasForeignKey(product => product.ProductCategoryId)
            .IsRequired();

        builder
            .Property(product => product.ProductCategoryId)
            .HasConversion(
                id => id.Value,
                value => ProductCategoryId.Create(value)
            )
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
    /// Configures the inventory table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureInventoryTable(EntityTypeBuilder<Product> builder)
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
    /// Configures the product discount table.
    /// </summary>
    /// <param name="builder">Then entity type builder.</param>
    private static void ConfigureProductDiscountTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(
            product => product.ProductDiscounts,
            productDiscountBuilder =>
            {
                productDiscountBuilder.ToTable("products_discounts");

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

                productDiscountBuilder
                    .HasOne<Discount>()
                    .WithMany()
                    .HasForeignKey(productDiscount => productDiscount.DiscountId)
                    .IsRequired();

                productDiscountBuilder
                    .Property(productDiscount => productDiscount.DiscountId)
                    .HasConversion(
                        id => id.Value,
                        value => DiscountId.Create(value)
                    )
                    .IsRequired();
            });
    }

    /// <summary>
    /// Configures the product image table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureProductImageTable(EntityTypeBuilder<Product> builder)
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
