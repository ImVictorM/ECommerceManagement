using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.PaymentAggregate;

/// <summary>
/// Configure the tables related directly with the <see cref="Payment"/> aggregate.
/// </summary>
public class PaymentConfigurations : IEntityTypeConfiguration<Payment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value, value => PaymentId.Create(value))
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .HasOne<Order>()
            .WithOne()
            .HasForeignKey<Payment>(p => p.OrderId);

        builder
            .Property(p => p.OrderId)
            .HasConversion(id => id.Value, value => OrderId.Create(value))
            .IsRequired();

        builder
            .HasOne<PaymentStatus>()
            .WithMany()
            .HasForeignKey(p => p.PaymentStatusId)
            .IsRequired();
    }
}
