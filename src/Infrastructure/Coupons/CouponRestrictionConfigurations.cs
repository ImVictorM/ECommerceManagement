using Domain.CouponAggregate.Abstracts;
using Domain.CouponAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Coupons;

/// <summary>
/// Configures the <see cref="CouponRestriction"/> abstract using the Type-per-concrete strategy.
/// </summary>
public class CouponRestrictionConfigurations : IEntityTypeConfiguration<CouponRestriction>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CouponRestriction> builder)
    {
        builder.UseTpcMappingStrategy();

        builder
            .Property<long>("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey("id");

        builder
            .Property<CouponId>("id_coupon")
            .HasConversion(id => id.Value, value => CouponId.Create(value))
            .IsRequired();
    }
}
