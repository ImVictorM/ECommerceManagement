using Domain.ShipmentStatusAggregate;
using Domain.ShipmentStatusAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="ShipmentStatus"/> aggregate to entity framework.
/// </summary>
public sealed class ShipmentStatusConfigurations : IEntityTypeConfiguration<ShipmentStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ShipmentStatus> builder)
    {
        ConfigureShipmentStatusTable(builder);
    }

    /// <summary>
    /// Configure the shipment status table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureShipmentStatusTable(EntityTypeBuilder<ShipmentStatus> builder)
    {
        builder.ToTable("shipment_statuses");

        builder.HasKey(shipmentStatus => shipmentStatus.Id);

        builder
            .Property(shipmentStatus => shipmentStatus.Id)
            .HasConversion(
                id => id.Value,
                value => ShipmentStatusId.Create(value)
            )
            .IsRequired();

        builder
            .Property(shipmentStatus => shipmentStatus.Status)
            .HasMaxLength(120)
            .IsRequired();
    }
}
