using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumeration;
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
        ConfigurePaymentsTable(builder);
        ConfigureOwnedPaymentStatusHistoriesTable(builder);
    }

    private static void ConfigurePaymentsTable(EntityTypeBuilder<Payment> builder)
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
            .Property(payment => payment.Amount)
            .IsRequired();

        builder
            .Property(payment => payment.Installments);

        builder.Ignore(payment => payment.PaymentMethod);
    }

    private static void ConfigureOwnedPaymentStatusHistoriesTable(EntityTypeBuilder<Payment> builder)
    {
        builder.OwnsMany(p => p.PaymentStatusHistories, paymentStatusHistoryBuilder =>
        {
            paymentStatusHistoryBuilder.UsePropertyAccessMode(PropertyAccessMode.Field);

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
                .Property(psh => psh.CreatedAt)
                .IsRequired();
        });
    }
}
