using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using SharedKernel.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Payments;

internal sealed class PaymentConfigurations : EntityTypeConfigurationDependency<Payment>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value, value => PaymentId.Create(value))
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasKey(p => p.Id);

        builder
            .HasOne<Order>()
            .WithOne()
            .HasForeignKey<Payment>(p => p.OrderId);

        builder
            .Property(p => p.OrderId)
            .HasConversion(id => id.Value, value => OrderId.Create(value))
            .IsRequired();

        builder
            .Property<long>("_paymentStatusId")
            .HasColumnName("id_payment_status")
            .IsRequired();

        builder
            .HasOne<PaymentStatus>()
            .WithMany()
            .HasForeignKey("_paymentStatusId")
            .IsRequired();

        builder.Ignore(p => p.PaymentStatus);

        builder
            .Property(p => p.PaymentStatus)
            .HasConversion(status => status.Id, id => BaseEnumeration.FromValue<PaymentStatus>(id));
    }
}
