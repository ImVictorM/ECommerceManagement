using Domain.OrderAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.OrderAggregate;

/// <summary>
/// Configures the <see cref="OrderStatus"/> value object to its table.
/// </summary>
public sealed class OrderStatusConfigurations : IEntityTypeConfiguration<OrderStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("order_statuses");

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .Property(orderStatus => orderStatus.Name)
            .HasMaxLength(120)
            .IsRequired();
    }
}
