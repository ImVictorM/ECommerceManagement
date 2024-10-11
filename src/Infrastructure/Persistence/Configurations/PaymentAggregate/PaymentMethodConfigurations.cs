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

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .Property(paymentMethod => paymentMethod.Name)
            .HasMaxLength(120)
            .IsRequired();
    }
}
