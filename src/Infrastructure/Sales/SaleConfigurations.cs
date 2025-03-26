using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations;
using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Sales;

internal sealed class SaleConfigurations : EntityTypeConfigurationDependency<Sale>
{
    public override void Configure(EntityTypeBuilder<Sale> builder)
    {
        ConfigureSalesTable(builder);
        ConfigureOwnedSaleProductsTable(builder);
        ConfigureOwnedSaleExcludedProductsTable(builder);
        ConfigureOwnedSaleCategoriesTable(builder);
    }

    private static void ConfigureSalesTable(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("sales");

        builder.HasKey(s => s.Id);

        builder
            .Property(s => s.Id)
            .HasConversion(id => id.Value, value => SaleId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.OwnsOne(
            s => s.Discount,
            DiscountNavigationBuilderConfigurations.Configure
        );
    }

    private static void ConfigureOwnedSaleCategoriesTable(
        EntityTypeBuilder<Sale> builder
    )
    {
        builder.OwnsMany(s => s.CategoriesOnSale, saleCategoriesBuilder =>
        {
            saleCategoriesBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            saleCategoriesBuilder.ToTable("sale_categories_categories");

            saleCategoriesBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            saleCategoriesBuilder.HasKey("id");

            saleCategoriesBuilder
                .WithOwner()
                .HasForeignKey("id_sale");

            saleCategoriesBuilder
                .Property("id_sale")
                .IsRequired();

            saleCategoriesBuilder
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(s => s.CategoryId)
                .IsRequired();

            saleCategoriesBuilder
                .Property(s => s.CategoryId)
                .HasConversion(id => id.Value, value => CategoryId.Create(value))
                .IsRequired();
        });
    }

    private static void ConfigureOwnedSaleExcludedProductsTable(
        EntityTypeBuilder<Sale> builder
    )
    {
        builder.OwnsMany(s => s.ProductsExcludedFromSale, saleProductsExcludedBuilder =>
        {
            saleProductsExcludedBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            saleProductsExcludedBuilder.ToTable("sale_excluded_products_products");

            saleProductsExcludedBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            saleProductsExcludedBuilder.HasKey("id");

            saleProductsExcludedBuilder
                .WithOwner()
                .HasForeignKey("id_sale");

            saleProductsExcludedBuilder
                .Property("id_sale")
                .IsRequired();

            saleProductsExcludedBuilder
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .IsRequired();

            saleProductsExcludedBuilder
                .Property(s => s.ProductId)
                .HasConversion(id => id.Value, value => ProductId.Create(value))
                .IsRequired();
        });
    }

    private static void ConfigureOwnedSaleProductsTable(
        EntityTypeBuilder<Sale> builder
    )
    {
        builder.OwnsMany(s => s.ProductsOnSale, saleProductsBuilder =>
        {
            saleProductsBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            saleProductsBuilder.ToTable("sale_products_products");

            saleProductsBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            saleProductsBuilder.HasKey("id");

            saleProductsBuilder
                .WithOwner()
                .HasForeignKey("id_sale");

            saleProductsBuilder
                .Property("id_sale")
                .IsRequired();

            saleProductsBuilder
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .IsRequired();

            saleProductsBuilder
                .Property(s => s.ProductId)
                .HasConversion(id => id.Value, value => ProductId.Create(value))
                .IsRequired();
        });
    }
}
