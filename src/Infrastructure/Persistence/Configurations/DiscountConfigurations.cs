using Domain.DiscountAggregate;
using Domain.DiscountAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="Discount"/> aggregate to entity framework.
/// </summary>
public sealed class DiscountConfigurations : IEntityTypeConfiguration<Discount>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        ConfigureDiscountTable(builder);
    }

    /// <summary>
    /// Configure the discount table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureDiscountTable(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("discounts");

        builder.HasKey(discount => discount.Id);

        builder
            .Property(discount => discount.Id)
            .HasConversion(
                id => id.Value,
                value => DiscountId.Create(value)
            )
            .IsRequired();

        builder
            .Property(discount => discount.Percentage)
            .IsRequired();

        builder
            .Property(discount => discount.Description)
            .HasMaxLength(250);

        builder
            .Property(discount => discount.StartingDate)
            .IsRequired();

        builder
            .Property(discount => discount.EndingDate)
            .IsRequired();
    }
}
