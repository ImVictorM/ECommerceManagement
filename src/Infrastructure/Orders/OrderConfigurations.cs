using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using SharedKernel.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Orders;

internal sealed class OrderConfigurations : EntityTypeConfigurationDependency<Order>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<Order> builder)
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
            .Property<long>("_orderStatusId")
            .HasColumnName("id_order_status")
            .IsRequired();

        builder
            .HasOne<OrderStatus>()
            .WithMany()
            .HasForeignKey("_orderStatusId")
            .IsRequired();

        builder.Ignore(order => order.OrderStatus);

        builder
            .Property(order => order.OrderStatus)
            .HasConversion(status => status.Id, id => BaseEnumeration.FromValue<OrderStatus>(id));

        builder
            .Property(order => order.Total)
            .IsRequired();

        builder
            .Property(order => order.Description)
            .HasMaxLength(200)
            .IsRequired();
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
        builder.OwnsMany(o => o.OrderTrackingEntries, orderTrackingEntryBuilder =>
        {
            orderTrackingEntryBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            orderTrackingEntryBuilder.ToTable("order_tracking_entries");

            orderTrackingEntryBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd();

            orderTrackingEntryBuilder.HasKey("id");

            orderTrackingEntryBuilder
                .WithOwner()
                .HasForeignKey("id_order");

            orderTrackingEntryBuilder
                .Property("id_order")
                .IsRequired();

            orderTrackingEntryBuilder
                .Property<long>("_orderStatusId")
                .HasColumnName("id_order_status")
                .IsRequired();

            orderTrackingEntryBuilder
                .HasOne<OrderStatus>()
                .WithMany()
                .HasForeignKey("_orderStatusId")
                .IsRequired();

            orderTrackingEntryBuilder.Ignore(o => o.OrderStatus);

            orderTrackingEntryBuilder
                .Property(o => o.CreatedAt)
                .IsRequired();
        });
    }
}
