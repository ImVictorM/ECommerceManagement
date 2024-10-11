using Domain.InstallmentAggregate;
using Domain.InstallmentAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.InstallmentAggregate;

/// <summary>
/// Configure the tables related directly with the <see cref="Installment"/> aggregate.
/// </summary>
public sealed class InstallmentConfigurations : IEntityTypeConfiguration<Installment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Installment> builder)
    {
        ConfigureInstallmentTable(builder);
    }

    /// <summary>
    /// Configure the installment table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureInstallmentTable(EntityTypeBuilder<Installment> builder)
    {
        builder.ToTable("installments");

        builder.HasKey(installment => installment.Id);

        builder
            .Property(installment => installment.Id)
            .HasConversion(
                id => id.Value,
                value => InstallmentId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<Order>()
            .WithOne()
            .HasForeignKey<Installment>(installment => installment.OrderId);

        builder
            .Property(installment => installment.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            )
            .IsRequired();

        builder
            .Property(installment => installment.QuantityPayments)
            .IsRequired();

        builder
            .Property(installment => installment.AmountPerPayment)
            .IsRequired();
    }
}
