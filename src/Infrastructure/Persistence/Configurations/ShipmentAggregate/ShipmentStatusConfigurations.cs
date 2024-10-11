using Domain.ShipmentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.ShipmentAggregate;

/// <summary>
/// Configures the <see cref="ShipmentStatus"/> value object to its table.
/// </summary>
public sealed class ShipmentStatusConfigurations : IEntityTypeConfiguration<ShipmentStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ShipmentStatus> builder)
    {
        ConfigureShipmentStatusTable(builder);
    }

    /// <summary>
    /// Configure the shipment_statuses table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureShipmentStatusTable(EntityTypeBuilder<ShipmentStatus> builder)
    {
        builder.ToTable("shipment_statuses");

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .Property(shipmentStatus => shipmentStatus.Name)
            .HasMaxLength(120)
            .IsRequired();
    }
}
