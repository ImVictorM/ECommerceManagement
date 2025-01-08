using Domain.PaymentAggregate.Enumerations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.PaymentAggregate;

/// <summary>
/// Configures the <see cref="PaymentStatus"/> enumeration to its table.
/// </summary>
public class PaymentStatusConfigurations : IEntityTypeConfiguration<PaymentStatus>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        builder.ToTable("payment_statuses");

        builder.HasKey(ps => ps.Id);

        builder
            .Property(ps => ps.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(ps => ps.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(ps => ps.Name).IsUnique();

        builder.HasData(PaymentStatus.List());
    }
}
