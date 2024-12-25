using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.Entities;
using Domain.ShipmentAggregate.ValueObjects;
using Infrastructure.Persistence.Configurations.Common;
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
        ConfigureShipmentsTable(builder);
        ConfigureOwnedShipmentStatusHistoriesTable(builder);
    }

    private static void ConfigureShipmentsTable(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("shipments");

        builder.HasKey(shipment => shipment.Id);

        builder
            .Property(shipment => shipment.Id)
            .HasConversion(
                id => id.Value,
                value => ShipmentId.Create(value)
            )
            .ValueGeneratedOnAdd()
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
            .IsRequired();

        builder.OwnsOne(order => order.DeliveryAddress, AddressNavigationBuilderConfigurations.Configure);
    }

    private static void ConfigureOwnedShipmentStatusHistoriesTable(EntityTypeBuilder<Shipment> builder)
    {
        builder.OwnsMany(s => s.ShipmentStatusHistories, shipmentStatusHistoryBuilder =>
        {
            shipmentStatusHistoryBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            shipmentStatusHistoryBuilder.ToTable("shipment_status_histories");

            shipmentStatusHistoryBuilder.Property<long>("id").ValueGeneratedOnAdd();
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
                .IsRequired();

            shipmentStatusHistoryBuilder
                .Property(ssh => ssh.CreatedAt)
                .IsRequired();
        });
    }
}
