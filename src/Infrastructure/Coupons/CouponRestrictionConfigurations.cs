using Domain.CouponAggregate.Abstracts;
using Domain.CouponAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Coupons;

internal sealed class CouponRestrictionConfigurations
    : EntityTypeConfigurationDependency<CouponRestriction>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<CouponRestriction> builder)
    {
        builder.UseTpcMappingStrategy();

        builder
            .Property<long>("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey("id");

        // This property is configured in the CouponConfigurations as a foreign
        // key.
        builder
            .Property<CouponId>("id_coupon")
            .HasConversion(id => id.Value, value => CouponId.Create(value))
            .IsRequired();
    }
}
