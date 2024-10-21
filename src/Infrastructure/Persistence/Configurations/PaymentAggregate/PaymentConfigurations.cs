using Domain.InstallmentAggregate;
using Domain.InstallmentAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Entities;
using Domain.PaymentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.PaymentAggregate;

/// <summary>
/// Configure the tables related directly with the <see cref="Payment"/> aggregate.
/// </summary>
public sealed class PaymentConfigurations : IEntityTypeConfiguration<Payment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        ConfigurePaymentTable(builder);
        ConfigureOwnedPaymentStatusHistoryTable(builder);
    }

    /// <summary>
    /// Configure the payments table.
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
            .ValueGeneratedOnAdd()
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
            .HasForeignKey(p => p.PaymentStatusId)
            .IsRequired();

        builder
            .Property(p => p.PaymentStatusId)
            .HasConversion(
                id => id.Value,
                value => PaymentStatusId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(p => p.PaymentMethodId)
            .IsRequired();

        builder
            .Property(p => p.PaymentMethodId)
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
    /// Configures the payment_status_histories table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureOwnedPaymentStatusHistoryTable(EntityTypeBuilder<Payment> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Payment.PaymentStatusHistories))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(p => p.PaymentStatusHistories, paymentStatusHistoryBuilder =>
        {
            paymentStatusHistoryBuilder.ToTable("payment_status_histories");

            paymentStatusHistoryBuilder
                .Property<long>("id")
                .ValueGeneratedOnAdd();

            paymentStatusHistoryBuilder.HasKey("id");

            paymentStatusHistoryBuilder
                .WithOwner()
                .HasForeignKey("id_payment");

            paymentStatusHistoryBuilder
                .Property("id_payment")
                .IsRequired();

            paymentStatusHistoryBuilder
                .HasOne<PaymentStatus>()
                .WithMany()
                .HasForeignKey(psh => psh.PaymentStatusId)
                .IsRequired();

            paymentStatusHistoryBuilder
                .Property(psh => psh.PaymentStatusId)
                .HasConversion(
                    id => id.Value,
                    value => PaymentStatusId.Create(value)
                )
                .IsRequired();

            paymentStatusHistoryBuilder
                .Property(psh => psh.CreatedAt)
                .IsRequired();
        });
    }
}
