using Domain.ShipmentAggregate.Entities;
using Domain.ShipmentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.ShipmentAggregate;

/// <summary>
/// Configures the <see cref="ShipmentStatus"/> entity to its table.
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

        builder.HasKey(ss => ss.Id);

        builder
            .Property(ss => ss.Id)
            .HasConversion(
                id => id.Value,
                value => ShipmentStatusId.Create(value)
            )
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
