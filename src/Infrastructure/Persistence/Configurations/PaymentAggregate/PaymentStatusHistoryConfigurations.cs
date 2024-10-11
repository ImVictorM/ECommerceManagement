using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.PaymentAggregate;

/// <summary>
/// Configures the <see cref="PaymentMethod"/> value object to its table.
/// </summary>
public sealed class PaymentStatusHistoryConfigurations : IEntityTypeConfiguration<PaymentStatusHistory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentStatusHistory> builder)
    {
        builder.ToTable("payment_status_histories");

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .HasOne(psh => psh.PaymentStatus)
            .WithMany()
            .HasForeignKey("id_payment_status")
            .IsRequired();

        builder
            .HasOne<Payment>()
            .WithMany(p => p.PaymentStatusHistories)
            .HasForeignKey("id_payment")
            .IsRequired();

        builder
            .Property(psh => psh.CreatedAt)
            .IsRequired();
    }
}
