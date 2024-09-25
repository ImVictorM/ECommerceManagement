using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Map the <see cref="ProductFeedback"/> aggregate to entity framework.
/// </summary>
public sealed class ProductFeedbackConfigurations : IEntityTypeConfiguration<ProductFeedback>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ProductFeedback> builder)
    {
        ConfigureProductFeedbackTable(builder);
    }

    /// <summary>
    /// Configure the product feedback table.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    private static void ConfigureProductFeedbackTable(EntityTypeBuilder<ProductFeedback> builder)
    {
        builder.ToTable("product_feedbacks");

        builder.HasKey(productFeedback => productFeedback.Id);

        builder
            .Property(productFeedback => productFeedback.Id)
            .HasConversion(
                id => id.Value,
                value => ProductFeedbackId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(productFeedback => productFeedback.ProductId)
            .IsRequired();

        builder
            .Property(productFeedback => productFeedback.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(productFeedback => productFeedback.UserId)
            .IsRequired();

        builder
            .Property(productFeedback => productFeedback.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<Order>()
            .WithOne()
            .HasForeignKey<ProductFeedback>(productFeedback => productFeedback.OrderId)
            .IsRequired();

        builder
            .Property(productFeedback => productFeedback.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            )
            .IsRequired();

        builder
            .Property(productFeedback => productFeedback.Subject)
            .HasMaxLength(60)
            .IsRequired();

        builder
            .Property(productFeedback => productFeedback.Content)
            .IsRequired();

        builder
           .Property(productFeedback => productFeedback.StarRating);

        builder
           .Property(productFeedback => productFeedback.IsActive)
           .IsRequired();
    }
}
