using Domain.PaymentAggregate.Entities;
using Domain.PaymentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.PaymentAggregate;

/// <summary>
/// Configures the <see cref="PaymentStatus"/> entity to its table.
/// </summary>
public sealed class PaymentStatusConfigurations : IEntityTypeConfiguration<PaymentStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        builder.ToTable("payment_statuses");

        builder.HasKey(ps => ps.Id);

        builder
            .Property(ps => ps.Id)
            .HasConversion(
                id => id.Value,
                value => PaymentStatusId.Create(value)
            )
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(paymentStatus => paymentStatus.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(ps => ps.Name).IsUnique();

        builder.HasData(PaymentStatus.List());
    }
}
