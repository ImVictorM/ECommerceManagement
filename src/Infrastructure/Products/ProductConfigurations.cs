using Domain.CategoryAggregate;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Products;

internal sealed class ProductConfigurations : EntityTypeConfigurationDependency<Product>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        ConfigureProductsTable(builder);
        ConfigureOwnedInventoriesTable(builder);
        ConfigureOwnedProductsCategoriesTable(builder);
        ConfigureOwnedProductImagesTable(builder);
    }

    private static void ConfigureProductsTable(EntityTypeBuilder<Product> builder)
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

    private static void ConfigureOwnedProductsCategoriesTable(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(product => product.ProductCategories, productCategoryBuilder =>
        {
            productCategoryBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            productCategoryBuilder.ToTable("products_categories");

            productCategoryBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            productCategoryBuilder.HasKey("id");

            productCategoryBuilder.WithOwner().HasForeignKey("id_product");

            productCategoryBuilder
                .Property("id_product")
                .IsRequired();

            productCategoryBuilder
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired();
        });
    }

    private static void ConfigureOwnedInventoriesTable(EntityTypeBuilder<Product> builder)
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

    private static void ConfigureOwnedProductImagesTable(EntityTypeBuilder<Product> builder)
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
