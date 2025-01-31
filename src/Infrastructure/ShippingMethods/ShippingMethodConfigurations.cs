using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ShippingMethods;

/// <summary>
/// Configures the tables for the <see cref="ShippingMethod"/> aggregate.
/// </summary>
public class ShippingMethodConfigurations : IEntityTypeConfiguration<ShippingMethod>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ShippingMethod> builder)
    {
        builder.ToTable("shipping_methods");

        builder.HasKey(s => s.Id);

        builder
            .Property(s => s.Id)
            .HasConversion(id => id.Value, value => ShippingMethodId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(s => s.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(s => s.Amount)
            .IsRequired();

        builder
            .Property(s => s.EstimatedDeliveryDays)
            .IsRequired();
    }
}
