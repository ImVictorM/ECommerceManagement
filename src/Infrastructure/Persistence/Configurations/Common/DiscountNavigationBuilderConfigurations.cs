using Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Common;

/// <summary>
/// Represents the configurations for the discount owned type properties.
/// </summary>
public static class DiscountNavigationBuilderConfigurations
{
    /// <summary>
    /// Configures the common properties of an discount owned type.
    /// </summary>
    /// <typeparam name="T">The owner entity type.</typeparam>
    /// <param name="builder">The discount navigation builder.</param>
    public static void Configure<T>(OwnedNavigationBuilder<T, Discount> builder) where T : class
    {
        builder
            .Property(discount => discount.Percentage)
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
