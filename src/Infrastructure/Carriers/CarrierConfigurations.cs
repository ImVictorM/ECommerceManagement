using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Carriers;

/// <summary>
/// Configures the tables for the <see cref="Carrier"/> aggregate.
/// </summary>
public class CarrierConfigurations : IEntityTypeConfiguration<Carrier>
{
    /// <inheritdoc/>
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Carrier> builder)
    {
        builder.ToTable("carriers");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .HasConversion(id => id.Value, value => CarrierId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(c => c.Name)
            .HasMaxLength(120)
            .IsRequired();
    }
}
