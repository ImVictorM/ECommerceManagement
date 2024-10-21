using Domain.PaymentAggregate.Entities;
using Domain.PaymentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.PaymentAggregate;

/// <summary>
/// Configures the <see cref="PaymentMethod"/> value object to its table.
/// </summary>
public sealed class PaymentMethodConfigurations : IEntityTypeConfiguration<PaymentMethod>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods");

        builder.HasKey(pm => pm.Id);

        builder
            .Property(pm => pm.Id)
            .HasConversion(
                id => id.Value,
                value => PaymentMethodId.Create(value)
            )
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(paymentMethod => paymentMethod.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(pm => pm.Name).IsUnique();

        builder.HasData(PaymentMethod.List());
    }
}
