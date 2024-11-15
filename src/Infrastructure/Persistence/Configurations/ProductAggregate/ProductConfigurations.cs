using Domain.ProductAggregate;
using Domain.ProductAggregate.Enumerations;
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
        ConfigureOwnedProductCategoryTable(builder);
        ConfigureOwnedInventoryTable(builder);
        ConfigureOwnedProductDiscountTable(builder);
        ConfigureOwnedProductImageTable(builder);
    }

    /// <summary>
    /// Configures the products table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureProductTable(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(product => product.Id);

        builder
            .Property(product => product.Id)
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value)
            )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(product => product.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(product => product.Description)
            .HasMaxLength(255);

        builder
           .Property(product => product.BasePrice)
           .IsRequired();

        builder
           .Property(product => product.IsActive)
           .IsRequired();
    }

    /// <summary>
    /// Configures the many-to-many relationship between products and categories
    /// creating the products_categories table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedProductCategoryTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.ProductCategories, productCategoryBuilder =>
        {
            productCategoryBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            productCategoryBuilder.ToTable("products_categories");

            productCategoryBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            productCategoryBuilder.HasKey("id");

            productCategoryBuilder.WithOwner().HasForeignKey("id_product");

            productCategoryBuilder.Property("id_product").IsRequired();

            productCategoryBuilder
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired();
        });
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
                    .ValueGeneratedOnAdd()
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
            product => product.Discounts,
            productDiscountBuilder =>
            {
                productDiscountBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

                productDiscountBuilder.ToTable("product_discounts");

                productDiscountBuilder
                    .Property<long>("id")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                productDiscountBuilder.HasKey("id");


                productDiscountBuilder.WithOwner().HasForeignKey("id_product");

                productDiscountBuilder
                    .Property("id_product")
                    .IsRequired();

                DiscountNavigationBuilderConfigurations.Configure(productDiscountBuilder);
            });
    }

    /// <summary>
    /// Configures the product_images table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedProductImageTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(
            product => product.ProductImages,
            productImageBuilder =>
            {
                productImageBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

                productImageBuilder.ToTable("product_images");

                productImageBuilder
                    .Property<long>("id")
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                productImageBuilder.HasKey("id");

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
