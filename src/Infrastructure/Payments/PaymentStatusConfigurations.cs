using Domain.PaymentAggregate.Enumerations;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Payments;

internal sealed class PaymentStatusConfigurations
    : EntityTypeConfigurationDependency<PaymentStatus>
{
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
