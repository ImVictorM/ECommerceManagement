using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ShippingMethods;

internal sealed class ShippingMethodConfigurations : EntityTypeConfigurationDependency<ShippingMethod>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<ShippingMethod> builder)
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
            .Property(s => s.Price)
            .IsRequired();

        builder
            .Property(s => s.EstimatedDeliveryDays)
            .IsRequired();
    }
}
