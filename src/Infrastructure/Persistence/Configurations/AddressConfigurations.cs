using Domain.AddressAggregate;
using Domain.AddressAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="Address"/> aggregate to entity framework.
/// </summary>
public sealed class AddressConfigurations : IEntityTypeConfiguration<Address>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        ConfigureAddressTable(builder);
    }

    /// <summary>
    /// Configure the Address table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureAddressTable(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");

        builder.HasKey(address => address.Id);

        builder
            .Property(address => address.Id)
            .HasConversion(
                id => id.Value,
                value => AddressId.Create(value)
            );

        builder
            .Property(address => address.PostalCode)
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(address => address.Street)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(address => address.Neighborhood)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(address => address.State)
            .HasMaxLength(120)
            .IsRequired();

        builder
            .Property(address => address.City)
            .HasMaxLength(120)
            .IsRequired();
    }
}
