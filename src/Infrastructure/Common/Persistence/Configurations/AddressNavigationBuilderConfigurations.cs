using SharedKernel.ValueObjects;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Common.Persistence.Configurations;

internal static class AddressNavigationBuilderConfigurations
{
    public static void Configure<T>(OwnedNavigationBuilder<T, Address> builder)
        where T : class
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
