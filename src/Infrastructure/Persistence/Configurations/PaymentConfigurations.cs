using Domain.InstallmentAggregate;
using Domain.InstallmentAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.PaymentMethodAggregate;
using Domain.PaymentMethodAggregate.ValueObjects;
using Domain.PaymentStatusAggregate;
using Domain.PaymentStatusAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="Payment"/> aggregate to entity framework.
/// </summary>
public sealed class PaymentConfigurations : IEntityTypeConfiguration<Payment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        ConfigurePaymentTable(builder);
        ConfigurePaymentStatusHistoryTable(builder);
    }

    /// <summary>
    /// Configure the payment table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigurePaymentTable(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(payment => payment.Id);

        builder
            .Property(payment => payment.Id)
            .HasConversion(
                id => id.Value,
                value => PaymentId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<Installment>()
            .WithMany()
            .HasForeignKey(payment => payment.InstallmentId);

        builder
            .Property(payment => payment.InstallmentId)
            .HasConversion(
                id => id != null ? id.Value : (long?)null,
                value => value != null ? InstallmentId.Create((long)value) : null
            );

        builder
            .HasOne<Order>()
            .WithOne()
            .HasForeignKey<Payment>(payment => payment.OrderId)
            .IsRequired();

        builder
            .Property(payment => payment.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<PaymentStatus>()
            .WithMany()
            .HasForeignKey(payment => payment.PaymentStatusId)
            .IsRequired();

        builder
            .Property(payment => payment.PaymentStatusId)
            .HasConversion(
                id => id.Value,
                value => PaymentStatusId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(payment => payment.PaymentMethodId)
            .IsRequired();

        builder
            .Property(payment => payment.PaymentMethodId)
            .HasConversion(
                id => id.Value,
                value => PaymentMethodId.Create(value)
            )
            .IsRequired();

        builder
            .Property(payment => payment.Amount)
            .IsRequired();
    }

    /// <summary>
    /// Configure the payment status history table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigurePaymentStatusHistoryTable(EntityTypeBuilder<Payment> builder)
    {
        builder.OwnsMany(
            payment => payment.PaymentStatusHistories,
            paymentStatusHistoryBuilder =>
            {
                paymentStatusHistoryBuilder.ToTable("payment_status_histories");

                paymentStatusHistoryBuilder.HasKey(paymentStatusHistory => paymentStatusHistory.Id);

                paymentStatusHistoryBuilder
                    .Property(paymentStatusHistory => paymentStatusHistory.Id)
                    .HasConversion(
                        id => id.Value,
                        value => PaymentStatusHistoryId.Create(value)
                    )
                    .IsRequired();

                paymentStatusHistoryBuilder.WithOwner().HasForeignKey("id_payment");

                paymentStatusHistoryBuilder
                    .Property("id_payment")
                    .IsRequired();

                paymentStatusHistoryBuilder
                    .HasOne<PaymentStatus>()
                    .WithMany()
                    .HasForeignKey(paymentStatusHistory => paymentStatusHistory.PaymentStatusId)
                    .IsRequired();

                paymentStatusHistoryBuilder
                    .Property(paymentStatusHistory => paymentStatusHistory.PaymentStatusId)
                    .HasConversion(
                        id => id.Value,
                        value => PaymentStatusId.Create(value)
                    )
                    .IsRequired();
            });
    }
}
