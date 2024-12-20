using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponRestrictionAggregate;
using Domain.CouponRestrictionAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.CouponRestrictionAggregate;

/// <summary>
/// Configures the <see cref="CouponRestriction"/> using the Type-per-concrete strategy.
/// </summary>
public class CouponRestrictionConfigurations : IEntityTypeConfiguration<CouponRestriction>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CouponRestriction> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.HasKey(cr => cr.Id);

        builder
            .Property(cr => cr.Id)
            .HasConversion(id => id.Value, value => CouponRestrictionId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .HasOne<Coupon>()
            .WithMany()
            .HasForeignKey(cr => cr.CouponId);

        builder
            .Property(cr => cr.CouponId)
            .HasConversion(id => id.Value, value => CouponId.Create(value))
            .IsRequired();
    }
}
