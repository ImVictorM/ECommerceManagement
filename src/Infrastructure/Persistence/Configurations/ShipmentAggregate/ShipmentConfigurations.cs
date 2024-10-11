using Domain.OrderAggregate;
using Domain.ShipmentAggregate;
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
            .HasOne(s => s.ShipmentStatus)
            .WithMany()
            .HasForeignKey("id_shipment_status")
            .IsRequired();
    }
}
