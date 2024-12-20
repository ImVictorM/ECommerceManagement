using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations.CouponAggregate;

/// <summary>
/// Configures the <see cref="Coupon"/> tables.
/// </summary>
public class CouponConfigurations : IEntityTypeConfiguration<Coupon>
{
    /// <inheritdoc/>
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("coupons");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .HasConversion(id => id.Value, value => CouponId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.OwnsOne(c => c.Discount, DiscountNavigationBuilderConfigurations.Configure);

        builder
            .Property(c => c.Code)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(c => c.Code).IsUnique();

        builder.
            Property(c => c.UsageLimit)
            .IsRequired();

        builder
            .Property(c => c.MinPrice)
            .IsRequired();

        builder
            .Property(c => c.AutoApply)
            .IsRequired();

        builder
            .Property(c => c.IsActive)
            .IsRequired();
    }
}
