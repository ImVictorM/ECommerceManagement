using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;
using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations;
using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Shipments;

internal sealed class ShipmentConfigurations : EntityTypeConfigurationDependency<Shipment>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<Shipment> builder)
    {
        ConfigureShipmentsTable(builder);
        ConfigureOwnedShipmentTrackingEntriesTable(builder);
    }

    private static void ConfigureShipmentsTable(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("shipments");

        // Configure PK
        builder.HasKey(shipment => shipment.Id);

        builder
            .Property(shipment => shipment.Id)
            .HasConversion(
                id => id.Value,
                value => ShipmentId.Create(value)
            )
            .ValueGeneratedOnAdd()
            .IsRequired();

        // Configure order
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

        // Configure carrier
        builder
            .HasOne<Carrier>()
            .WithMany()
            .HasForeignKey(s => s.CarrierId)
            .IsRequired();

        builder
            .Property(s => s.CarrierId)
            .HasConversion(id => id.Value, value => CarrierId.Create(value))
            .IsRequired();

        // Configure shipping method
        builder
            .HasOne<ShippingMethod>()
            .WithMany()
            .HasForeignKey(s => s.ShippingMethodId)
            .IsRequired();

        builder
            .Property(s => s.ShippingMethodId)
            .HasConversion(id => id.Value, value => ShippingMethodId.Create(value))
            .IsRequired();

        // Configure shipment status
        builder
            .Property("_shipmentStatusId")
            .HasColumnName("id_shipment_status")
            .IsRequired();

        builder
            .HasOne<ShipmentStatus>()
            .WithMany()
            .HasForeignKey("_shipmentStatusId")
            .IsRequired();

        builder.Ignore(s => s.ShipmentStatus);

        builder.OwnsOne(order => order.DeliveryAddress, AddressNavigationBuilderConfigurations.Configure);
    }

    private static void ConfigureOwnedShipmentTrackingEntriesTable(EntityTypeBuilder<Shipment> builder)
    {
        builder.OwnsMany(s => s.ShipmentTrackingEntries, shipmentTrackingEntriesBuilder =>
        {
            shipmentTrackingEntriesBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

            shipmentTrackingEntriesBuilder.ToTable("shipment_tracking_entries");

            shipmentTrackingEntriesBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd();

            shipmentTrackingEntriesBuilder.HasKey("id");

            shipmentTrackingEntriesBuilder
                .WithOwner()
                .HasForeignKey("id_shipment");

            shipmentTrackingEntriesBuilder
                .Property("id_shipment")
                .IsRequired();

            shipmentTrackingEntriesBuilder
                .Property<long>("_shipmentStatusId")
                .HasColumnName("id_shipment_status")
                .IsRequired();

            shipmentTrackingEntriesBuilder
                .HasOne<ShipmentStatus>()
                .WithMany()
                .HasForeignKey("_shipmentStatusId")
                .IsRequired();

            shipmentTrackingEntriesBuilder.Ignore(s => s.ShipmentStatus);

            shipmentTrackingEntriesBuilder
                .Property(s => s.CreatedAt)
                .IsRequired();
        });
    }
}
