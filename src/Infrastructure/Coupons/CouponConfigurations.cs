using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations;
using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Coupons;

internal sealed class CouponConfigurations : EntityTypeConfigurationDependency<Coupon>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("coupons");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .HasConversion(id => id.Value, value => CouponId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.OwnsOne(
            c => c.Discount,
            DiscountNavigationBuilderConfigurations.Configure
        );

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

        builder
            .HasMany(c => c.Restrictions)
            .WithOne()
            .HasForeignKey("id_coupon")
            .IsRequired();

        builder
            .Navigation(c => c.Restrictions)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .AutoInclude();
    }
}
