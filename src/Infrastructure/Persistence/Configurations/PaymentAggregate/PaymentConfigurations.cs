using Domain.InstallmentAggregate;
using Domain.InstallmentAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
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
            .HasOne(p => p.PaymentStatus)
            .WithMany()
            .HasForeignKey("id_payment_status")
            .IsRequired();

        builder
            .HasOne(p => p.PaymentMethod)
            .WithMany()
            .HasForeignKey("id_payment_method")
            .IsRequired();

        builder
            .Property(payment => payment.Amount)
            .IsRequired();
    }
}
