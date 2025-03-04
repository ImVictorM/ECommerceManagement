using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ProductFeedback;

internal sealed class ProductFeedbackConfigurations : EntityTypeConfigurationDependency<DomainProductFeedback>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<DomainProductFeedback> builder)
    {
        ConfigureProductFeedbacksTable(builder);
    }

    private static void ConfigureProductFeedbacksTable(EntityTypeBuilder<DomainProductFeedback> builder)
    {
        builder.ToTable("product_feedback");

        builder.HasKey(productFeedback => productFeedback.Id);

        builder
            .Property(productFeedback => productFeedback.Id)
            .HasConversion(
                id => id.Value,
                value => ProductFeedbackId.Create(value)
            )
            .ValueGeneratedOnAdd()
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
            .Property(productFeedback => productFeedback.Title)
            .HasMaxLength(60)
            .IsRequired();

        builder
            .Property(productFeedback => productFeedback.Content)
            .HasMaxLength(200)
            .IsRequired();

        builder
           .Property(productFeedback => productFeedback.StarRating)
           .HasConversion(
                rating => rating.Value,
                value => StarRating.Create(value)
           )
           .IsRequired();

        builder
           .Property(productFeedback => productFeedback.IsActive)
           .IsRequired();
    }
}
