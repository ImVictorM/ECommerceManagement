using Domain.OrderAggregate.Enumerations;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Orders;

/// <summary>
/// Configures the <see cref="OrderStatus"/> table.
/// </summary>
public sealed class OrderStatusConfigurations : EntityTypeConfigurationDependency<OrderStatus>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("order_statuses");

        builder.HasKey(os => os.Id);

        builder
            .Property(os => os.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(orderStatus => orderStatus.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(os => os.Name).IsUnique();

        builder.HasData(OrderStatus.List());
    }
}
