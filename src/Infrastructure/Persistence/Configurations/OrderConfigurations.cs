using Domain.AddressAggregate;
using Domain.AddressAggregate.ValueObjects;
using Domain.DiscountAggregate;
using Domain.DiscountAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Entities;
using Domain.OrderAggregate.ValueObjects;
using Domain.OrderStatusAggregate;
using Domain.OrderStatusAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="Order"/> aggregate to entity framework.
/// </summary>
public sealed class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ConfigureOrderTable(builder);
        ConfigureOrderStatusHistoryTable(builder);
        ConfigureOrderDiscountTable(builder);
        ConfigureOrderProductTable(builder);
    }

    /// <summary>
    /// Configures the order product table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOrderProductTable(EntityTypeBuilder<Order> builder)
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

    /// <summary>
    /// Configures the order discount table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOrderDiscountTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(
            order => order.OrderDiscounts,
            orderDiscountBuilder =>
            {
                orderDiscountBuilder.ToTable("orders_discounts");

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

                orderDiscountBuilder
                    .HasOne<Discount>()
                    .WithMany()
                    .HasForeignKey(orderDiscount => orderDiscount.DiscountId)
                    .IsRequired();

                orderDiscountBuilder
                    .Property(orderDiscount => orderDiscount.DiscountId)
                    .HasConversion(
                        id => id.Value,
                        value => DiscountId.Create(value)
                    )
                    .IsRequired();
            });
    }

    /// <summary>
    /// Configures the order status history change table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOrderStatusHistoryTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(
            order => order.OrderStatusHistories,
            orderStatusHistoryBuilder =>
            {
                orderStatusHistoryBuilder.ToTable("order_status_histories");

                orderStatusHistoryBuilder.HasKey(orderStatusHistory => orderStatusHistory.Id);

                orderStatusHistoryBuilder
                    .Property(orderStatusHistory => orderStatusHistory.Id)
                    .HasConversion(
                        id => id.Value,
                        value => OrderStatusHistoryId.Create(value)
                    )
                    .IsRequired();

                orderStatusHistoryBuilder.WithOwner().HasForeignKey("id_order");

                orderStatusHistoryBuilder
                    .Property("id_order")
                    .IsRequired();

                orderStatusHistoryBuilder
                    .HasOne<OrderStatus>()
                    .WithMany()
                    .HasForeignKey(orderStatusHistory => orderStatusHistory.OrderStatusId)
                    .IsRequired();

                orderStatusHistoryBuilder
                    .Property(orderStatusHistory => orderStatusHistory.OrderStatusId)
                    .HasConversion(
                        id => id.Value,
                        value => OrderStatusId.Create(value)
                    )
                    .IsRequired();
            });
    }

    /// <summary>
    /// Configures the order table.
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
            .HasOne<Address>()
            .WithMany()
            .HasForeignKey(order => order.AddressId)
            .IsRequired();

        builder
            .Property(order => order.AddressId)
            .HasConversion(
                id => id.Value,
                value => AddressId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<OrderStatus>()
            .WithMany()
            .HasForeignKey(order => order.OrderStatusId)
            .IsRequired();

        builder
            .Property(order => order.OrderStatusId)
            .HasConversion(
                id => id.Value,
                value => OrderStatusId.Create(value)
            )
            .IsRequired();

        builder
            .Property(order => order.Total)
            .IsRequired();
    }
}
