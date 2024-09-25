using Domain.PaymentMethodAggregate;
using Domain.PaymentMethodAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="PaymentMethod"/> aggregate to entity framework.
/// </summary>
public sealed class PaymentMethodConfigurations : IEntityTypeConfiguration<PaymentMethod>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        ConfigurePaymentMethodTable(builder);
    }

    /// <summary>
    /// Configure the payment method table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public static void ConfigurePaymentMethodTable(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods");

        builder.HasKey(paymentMethod => paymentMethod.Id);

        builder
            .Property(paymentMethod => paymentMethod.Id)
            .HasConversion(
                id => id.Value,
                value => PaymentMethodId.Create(value)
            )
            .IsRequired();

        builder
            .Property(paymentMethod => paymentMethod.Method)
            .HasMaxLength(120)
            .IsRequired();
    }
}
