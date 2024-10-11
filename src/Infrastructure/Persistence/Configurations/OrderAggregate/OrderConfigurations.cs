using Domain.OrderAggregate;
using Domain.OrderAggregate.Entities;
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
        ConfigureOrderTable(builder);
        ConfigureOwnedOrderDiscountTable(builder);
        ConfigureOwnedOrderProductTable(builder);
    }

    /// <summary>
    /// Configures the orders table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOrderTable(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(order => order.Id);

        builder
            .Property(order => order.Id)
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(order => order.UserId)
            .IsRequired();

        builder
            .Property(order => order.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne(o => o.OrderStatus)
            .WithMany()
            .HasForeignKey("id_order_status")
            .IsRequired();

        builder
            .Property(order => order.Total)
            .IsRequired();

        builder.OwnsOne(order => order.Address, AddressNavigationBuilderConfigurations.Configure);
    }

    /// <summary>
    /// Configures the order discount table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedOrderDiscountTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(
            order => order.OrderDiscounts,
            orderDiscountBuilder =>
            {
                orderDiscountBuilder.ToTable("order_discounts");

                orderDiscountBuilder.HasKey(orderDiscount => orderDiscount.Id);
                orderDiscountBuilder
                    .Property(orderDiscount => orderDiscount.Id)
                    .HasConversion(
                        id => id.Value,
                        value => OrderDiscountId.Create(value)
                    )
                    .IsRequired();

                orderDiscountBuilder.WithOwner().HasForeignKey("id_order");
                orderDiscountBuilder
                    .Property("id_order")
                    .IsRequired();

                orderDiscountBuilder.OwnsOne(od => od.Discount, DiscountNavigationBuilderConfigurations.Configure);
            });
    }

    /// <summary>
    /// Configures the order product table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedOrderProductTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(order => order.OrderProducts, orderProductsBuilder =>
        {
            orderProductsBuilder.ToTable("orders_products");

            orderProductsBuilder.HasKey(orderProduct => orderProduct.Id);

            orderProductsBuilder
                .Property(orderProduct => orderProduct.Id)
                .HasConversion(
                    id => id.Value,
                    value => OrderProductId.Create(value)
                )
                .IsRequired();

            orderProductsBuilder.WithOwner().HasForeignKey("id_order");
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
                .Property(orderProduct => orderProduct.PriceOnOrder)
                .IsRequired();

            orderProductsBuilder
                .Property(OrderProduct => OrderProduct.Quantity)
                .IsRequired();
        });
    }
}