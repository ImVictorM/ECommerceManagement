using Domain.OrderStatusAggregate;
using Domain.OrderStatusAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="OrderStatus"/> aggregate to entity framework.
/// </summary>
public sealed class OrderStatusConfigurations : IEntityTypeConfiguration<OrderStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        ConfigureOrderStatusTable(builder);
    }

    /// <summary>
    /// Configures the order status table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public static void ConfigureOrderStatusTable(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("order_statuses");

        builder.HasKey(orderStatus => orderStatus.Id);

        builder
            .Property(orderStatus => orderStatus.Id)
            .HasConversion(
                id => id.Value,
                value => OrderStatusId.Create(value)
            )
            .IsRequired();

        builder
            .Property(orderStatus => orderStatus.Status)
            .HasMaxLength(120)
            .IsRequired();
    }
}
