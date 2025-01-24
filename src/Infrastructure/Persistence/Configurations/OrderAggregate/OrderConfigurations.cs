using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.OrderAggregate;

/// <summary>
/// Configure the tables related directly with the <see cref="Order"/> aggregate.
/// </summary>
public sealed class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ConfigureOrdersTable(builder);
        ConfigureOwnedOrdersCouponsTable(builder);
        ConfigureOwnedOrdersProductsTable(builder);
        ConfigureOwnedOrderStatusHistoriesTable(builder);
    }

    private static void ConfigureOrdersTable(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(order => order.Id);

        builder
            .Property(order => order.Id)
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(order => order.OwnerId)
            .IsRequired();

        builder
            .Property(order => order.OwnerId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<OrderStatus>()
            .WithMany()
            .HasForeignKey(o => o.OrderStatusId)
            .IsRequired();

        builder
            .Property(order => order.Total)
            .IsRequired();

        builder
            .Property(order => order.Description)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(order => order.DeliveryAddress, AddressNavigationBuilderConfigurations.Configure);
    }

    private static void ConfigureOwnedOrdersCouponsTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(o => o.CouponsApplied, orderCouponBuilder =>
        {
            orderCouponBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            orderCouponBuilder.ToTable("orders_coupons");

            orderCouponBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            orderCouponBuilder.HasKey("id");

            orderCouponBuilder
                .WithOwner()
                .HasForeignKey("id_order");

            orderCouponBuilder
                .Property("id_order")
                .IsRequired();

            orderCouponBuilder
                .HasOne<Coupon>()
                .WithMany()
                .HasForeignKey(orderCoupon => orderCoupon.CouponId)
                .IsRequired();

            orderCouponBuilder
                .Property(orderCoupon => orderCoupon.CouponId)
                .HasConversion(id => id.Value, value => CouponId.Create(value))
                .IsRequired();
        });
    }

    private static void ConfigureOwnedOrdersProductsTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(order => order.Products, orderProductsBuilder =>
        {
            orderProductsBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            orderProductsBuilder.ToTable("orders_products");

            orderProductsBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            orderProductsBuilder.HasKey("id");

            orderProductsBuilder
                .WithOwner()
                .HasForeignKey("id_order");

            orderProductsBuilder
                .Property("id_order")
                .IsRequired();

            orderProductsBuilder
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(orderProduct => orderProduct.ProductId)
                .IsRequired();

            orderProductsBuilder
                .Property(orderProduct => orderProduct.ProductId)
                .HasConversion(
                    id => id.Value,
                    value => ProductId.Create(value)
                )
                .IsRequired();

            orderProductsBuilder
                .Property(OrderProduct => OrderProduct.Quantity)
                .IsRequired();

            orderProductsBuilder
                .Property(orderProduct => orderProduct.BasePrice)
                .IsRequired();

            orderProductsBuilder
                .Property(orderProduct => orderProduct.PurchasedPrice)
                .IsRequired();

            orderProductsBuilder
                .OwnsMany(orderProduct => orderProduct.ProductCategoryIds, categoryBuilder =>
                {
                    categoryBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

                    categoryBuilder.ToTable("order_product_category_ids");

                    categoryBuilder
                        .Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .IsRequired();

                    categoryBuilder.HasKey("id");

                    categoryBuilder
                        .Property(categoryId => categoryId.Value)
                        .HasColumnName("id_category")
                        .IsRequired();

                    categoryBuilder
                        .WithOwner()
                        .HasForeignKey("id_order_product");

                    categoryBuilder
                        .Property("id_order_product")
                        .IsRequired();
                });
        });
    }

    private static void ConfigureOwnedOrderStatusHistoriesTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(o => o.OrderStatusHistories, orderStatusHistoryBuilder =>
        {
            orderStatusHistoryBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            orderStatusHistoryBuilder.ToTable("order_status_histories");

            orderStatusHistoryBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd();

            orderStatusHistoryBuilder.HasKey("id");

            orderStatusHistoryBuilder
                .WithOwner()
                .HasForeignKey("id_order");

            orderStatusHistoryBuilder
                .Property("id_order")
                .IsRequired();

            orderStatusHistoryBuilder
                .HasOne<OrderStatus>()
                .WithMany()
                .HasForeignKey(osh => osh.OrderStatusId)
                .IsRequired();

            orderStatusHistoryBuilder
                .Property(osh => osh.CreatedAt)
                .IsRequired();
        });
    }
}
