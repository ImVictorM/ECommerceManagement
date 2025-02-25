using SharedKernel.ValueObjects;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

internal static class DiscountNavigationBuilderConfigurations
{
    /// <inheritdoc/>
    public static void Configure<T>(OwnedNavigationBuilder<T, Discount> builder) where T : class
    {
        builder
            .Property(discount => discount.Percentage)
            .HasConversion(p => p.Value, value => Percentage.Create(value))
            .IsRequired();

        builder
            .Property(discount => discount.Description)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(discount => discount.StartingDate)
            .IsRequired();

        builder
            .Property(discount => discount.EndingDate)
            .IsRequired();
    }
}
