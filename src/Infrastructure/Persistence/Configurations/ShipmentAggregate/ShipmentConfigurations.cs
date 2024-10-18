using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.Entities;
using Domain.ShipmentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.ShipmentAggregate;

/// <summary>
/// Configure the tables related directly with the <see cref="Shipment"/> aggregate.
/// </summary>
public sealed class ShipmentConfigurations : IEntityTypeConfiguration<Shipment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        ConfigureShipmentTable(builder);
        ConfigureOwnedShipmentStatusHistoryTable(builder);
    }

    /// <summary>
    /// Configures the shipments table.
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
            .Property(s => s.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<ShipmentStatus>()
            .WithMany()
            .HasForeignKey(s => s.ShipmentStatusId)
            .IsRequired();

        builder
            .Property(s => s.ShipmentStatusId)
            .HasConversion(
                id => id.Value,
                value => ShipmentStatusId.Create(value)
            )
            .IsRequired();
    }

    /// <summary>
    /// Configures the shipment_status_histories table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedShipmentStatusHistoryTable(EntityTypeBuilder<Shipment> builder)
    {
        builder.OwnsMany(s => s.ShipmentStatusHistories, shipmentStatusHistoryBuilder =>
        {
            shipmentStatusHistoryBuilder.ToTable("shipment_status_histories");

            shipmentStatusHistoryBuilder.Property<long>("id");
            shipmentStatusHistoryBuilder.HasKey("id");

            shipmentStatusHistoryBuilder.WithOwner().HasForeignKey("id_shipment");

            shipmentStatusHistoryBuilder
                .Property("id_shipment")
                .IsRequired();

            shipmentStatusHistoryBuilder
                .HasOne<ShipmentStatus>()
                .WithMany()
                .HasForeignKey(ssh => ssh.ShipmentStatusId)
                .IsRequired();

            shipmentStatusHistoryBuilder
                .Property(ssh => ssh.ShipmentStatusId)
                .HasConversion(
                    id => id.Value,
                    value => ShipmentStatusId.Create(value)
                )
                .IsRequired();

            shipmentStatusHistoryBuilder
                .Property(ssh => ssh.CreatedAt)
                .IsRequired();
        });
    }
}
