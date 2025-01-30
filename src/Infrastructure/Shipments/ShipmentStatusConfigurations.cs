using Domain.ShipmentAggregate.Enumerations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Shipments;

/// <summary>
/// Configures the <see cref="ShipmentStatus"/> table.
/// </summary>
public sealed class ShipmentStatusConfigurations : IEntityTypeConfiguration<ShipmentStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ShipmentStatus> builder)
    {
        ConfigureShipmentStatusesTable(builder);
    }

    private static void ConfigureShipmentStatusesTable(EntityTypeBuilder<ShipmentStatus> builder)
    {
        builder.ToTable("shipment_statuses");

        builder.HasKey(ss => ss.Id);

        builder
            .Property(ss => ss.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(shipmentStatus => shipmentStatus.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(ss => ss.Name).IsUnique();

        builder.HasData(ShipmentStatus.List());
    }
}
