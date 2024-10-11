using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.ShipmentAggregate;

/// <summary>
/// Configures the <see cref="ShipmentStatusHistory"/> value object to its table.
/// </summary>
public sealed class ShipmentStatusHistoryConfigurations : IEntityTypeConfiguration<ShipmentStatusHistory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ShipmentStatusHistory> builder)
    {
        builder.ToTable("shipment_status_histories");

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .HasOne(ssh => ssh.ShipmentStatus)
            .WithMany()
            .HasForeignKey("id_shipment_status")
            .IsRequired();

        builder
            .HasOne<Shipment>()
            .WithMany(s => s.ShipmentStatusHistories)
            .HasForeignKey("id_shipment")
            .IsRequired();
    }
}
