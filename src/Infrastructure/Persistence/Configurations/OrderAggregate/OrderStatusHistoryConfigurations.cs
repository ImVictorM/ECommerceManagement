using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.OrderAggregate;

/// <summary>
/// Configures the <see cref="OrderStatusHistory"/> value object to its table.
/// </summary>
public sealed class OrderStatusHistoryConfigurations : IEntityTypeConfiguration<OrderStatusHistory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder.ToTable("order_status_histories");

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .HasOne<Order>()
            .WithMany(o => o.OrderStatusHistories)
            .HasForeignKey("id_order")
            .IsRequired();

        builder
            .HasOne(osh => osh.OrderStatus)
            .WithMany()
            .HasForeignKey("id_order_status")
            .IsRequired();

        builder
            .Property(osh => osh.CreatedAt)
            .IsRequired();
    }
}
