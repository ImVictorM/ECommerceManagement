using Domain.PaymentStatusAggregate;
using Domain.PaymentStatusAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="PaymentStatus"/> aggregate to entity framework.
/// </summary>
public sealed class PaymentStatusConfigurations : IEntityTypeConfiguration<PaymentStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        ConfigurePaymentStatusTable(builder);
    }

    /// <summary>
    /// Configures the payment status table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public static void ConfigurePaymentStatusTable(EntityTypeBuilder<PaymentStatus> builder)
    {
        builder.ToTable("payment_statuses");

        builder.HasKey(paymentStatus => paymentStatus.Id);

        builder
            .Property(paymentStatus => paymentStatus.Id)
            .HasConversion(
                id => id.Value,
                value => PaymentStatusId.Create(value)
            )
            .IsRequired();

        builder
            .Property(paymentStatus => paymentStatus.Status)
            .HasMaxLength(120)
            .IsRequired();
    }
}
