using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using Domain.ProductReviewAggregate.ValueObjects;
using Domain.ProductReviewAggregate;

using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ProductReviews;

internal sealed class ProductReviewConfigurations
    : EntityTypeConfigurationDependency<ProductReview>
{
    public override void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        ConfigureProductReviewsTable(builder);
    }

    private static void ConfigureProductReviewsTable(
        EntityTypeBuilder<ProductReview> builder
    )
    {
        builder.ToTable("product_reviews");

        builder.HasKey(review => review.Id);

        builder
            .Property(review => review.Id)
            .HasConversion(
                id => id.Value,
                value => ProductReviewId.Create(value)
            )
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(review => review.ProductId)
            .IsRequired();

        builder
            .Property(review => review.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value)
            )
            .IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(review => review.UserId)
            .IsRequired();

        builder
            .Property(review => review.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            )
            .IsRequired();

        builder
            .Property(review => review.Title)
            .HasMaxLength(60)
            .IsRequired();

        builder
            .Property(review => review.Content)
            .HasMaxLength(200)
            .IsRequired();

        builder
           .Property(review => review.StarRating)
           .HasConversion(
                rating => rating.Value,
                value => StarRating.Create(value)
           )
           .IsRequired();

        builder
           .Property(review => review.IsActive)
           .IsRequired();
    }
}
