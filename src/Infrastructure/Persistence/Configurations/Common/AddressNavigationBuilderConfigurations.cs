using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.ValueObjects;

namespace Infrastructure.Persistence.Configurations.Common;

/// <summary>
/// Represents the configurations for the address owned type properties.
/// </summary>
public static class AddressNavigationBuilderConfigurations
{
    /// <summary>
    /// Configures the common properties of an Address owned type.
    /// </summary>
    /// <typeparam name="T">The owner entity type.</typeparam>
    /// <param name="builder">The address navigation builder.</param>
    public static void Configure<T>(OwnedNavigationBuilder<T, Address> builder) where T : class
    {
        builder
            .Property(a => a.PostalCode)
            .HasMaxLength(10)
            .IsRequired();
        builder
            .Property(a => a.Street)
            .HasMaxLength(120)
            .IsRequired();
        builder
            .Property(a => a.Neighborhood)
            .HasMaxLength(120);
        builder
            .Property(a => a.State)
            .HasMaxLength(120)
            .IsRequired();
        builder
            .Property(a => a.City)
            .HasMaxLength(120)
            .IsRequired();
    }
}
