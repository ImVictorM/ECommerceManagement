using Domain.PaymentAggregate.Enumerations;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Payments;

/// <summary>
/// Configures the <see cref="PaymentStatus"/> table.
/// </summary>
public class PaymentStatusConfigurations : EntityTypeConfigurationDependency<PaymentStatus>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<PaymentStatus> builder)
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
