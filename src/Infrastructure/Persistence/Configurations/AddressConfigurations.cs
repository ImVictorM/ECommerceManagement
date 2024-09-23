using Domain.AddressAggregate;
using Domain.AddressAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class AddressConfigurations : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");

        builder.HasKey(address => address.Id).HasName(nameof(Address.Id).ToLowerInvariant());

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
