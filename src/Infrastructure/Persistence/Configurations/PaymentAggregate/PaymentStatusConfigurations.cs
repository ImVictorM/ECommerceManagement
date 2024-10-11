using Domain.PaymentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.PaymentAggregate;

/// <summary>
/// Configures the <see cref="PaymentStatus"/> value object to its table.
/// </summary>
public sealed class PaymentStatusConfigurations : IEntityTypeConfiguration<PaymentStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        builder.ToTable("payment_statuses");

        builder.Property<long>("id");
        builder.HasKey("id");

        builder
            .Property(paymentStatus => paymentStatus.Name)
            .HasMaxLength(120)
            .IsRequired();
    }
}
