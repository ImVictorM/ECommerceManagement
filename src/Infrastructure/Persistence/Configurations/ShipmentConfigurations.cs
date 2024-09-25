using Domain.OrderAggregate;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.ShipmentStatusAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="Shipment"/> aggregate to entity framework.
/// </summary>
public sealed class ShipmentConfigurations : IEntityTypeConfiguration<Shipment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        ConfigureShipmentTable(builder);
        ConfigureShipmentStatusHistoryTable(builder);
    }

    /// <summary>
    /// Configures the shipment table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureShipmentTable(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("shipments");

        builder.HasKey(shipment => shipment.Id);

        builder
            .Property(shipment => shipment.Id)
            .HasConversion(
                id => id.Value,
                value => ShipmentId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<Order>()
            .WithOne()
            .HasForeignKey<Shipment>(shipment => shipment.OrderId)
            .IsRequired();

        builder
            .HasOne<ShipmentStatus>()
            .WithMany()
            .HasForeignKey(shipment => shipment.ShipmentStatusId)
            .IsRequired();
    }

    /// <summary>
    /// Configures the shipment status history table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureShipmentStatusHistoryTable(EntityTypeBuilder<Shipment> builder)
    {
        builder.OwnsMany(
            shipment => shipment.ShipmentStatusHistories,
            shipmentStatusHistoryBuilder =>
            {
                shipmentStatusHistoryBuilder.ToTable("shipment_status_histories");

                shipmentStatusHistoryBuilder.HasKey(shipmentStatusHistory => shipmentStatusHistory.Id);

                shipmentStatusHistoryBuilder
                    .Property(shipmentStatusHistory => shipmentStatusHistory.Id)
                    .HasConversion(
                        id => id.Value,
                        value => ShipmentStatusHistoryId.Create(value)
                    )
                    .IsRequired();

                shipmentStatusHistoryBuilder.WithOwner().HasForeignKey("id_shipment");

                shipmentStatusHistoryBuilder
                    .Property("id_shipment")
                    .IsRequired();

                shipmentStatusHistoryBuilder
                    .HasOne<ShipmentStatus>()
                    .WithMany()
                    .HasForeignKey(shipmentStatusHistory => shipmentStatusHistory.ShipmentStatusId)
                    .IsRequired();
            });
    }
}
