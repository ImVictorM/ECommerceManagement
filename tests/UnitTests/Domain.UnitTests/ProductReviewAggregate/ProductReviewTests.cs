using Domain.ProductAggregate.ValueObjects;
using Domain.ProductReviewAggregate;
using Domain.ProductReviewAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

namespace Domain.UnitTests.ProductReviewAggregate;

/// <summary>
/// Unit tests for the <see cref="ProductReview"/> aggregate root.
/// </summary>
public class ProductReviewTests
{
    /// <summary>
    /// Provides a list containing valid parameters to create a new product review.
    /// </summary>
    public static readonly IEnumerable<object[]> CreateProductReviewValidParameters =
    [
        [
            UserId.Create(1),
            ProductId.Create(1),
            "Good Guitar",
            "It sounds clean and angelical",
            StarRating.Create(5)
        ],
        [
            UserId.Create(4),
            ProductId.Create(2),
            "Poor Pillow Quality",
            "I feel like sleeping on a rock",
            StarRating.Create(1)
        ],
    ];

    /// <summary>
    /// Verifies it is possible to create a product review with valid parameters.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="productId">The product identifier.</param>
    /// <param name="title">The review title.</param>
    /// <param name="content">The review content.</param>
    /// <param name="starRating">The review star rating.</param>
    [Theory]
    [MemberData(nameof(CreateProductReviewValidParameters))]
    public void Create_WithValidParameters_CreatesWithoutThrowing(
        UserId userId,
        ProductId productId,
        string title,
        string content,
        StarRating starRating
    )
    {
        var action = FluentActions
            .Invoking(() => ProductReview.Create(
                userId: userId,
                productId: productId,
                title: title,
                content: content,
                starRating: starRating
            ))
            .Should()
            .NotThrow();

        var review = action.Subject;

        review.UserId.Should().Be(userId);
        review.ProductId.Should().Be(productId);
        review.Title.Should().Be(title);
        review.Content.Should().Be(content);
        review.StarRating.Should().Be(starRating);
        review.IsActive.Should().BeTrue();
    }

    /// <summary>
    /// Verifies deactivating the product review makes the review inactive
    /// by setting the <see cref="ProductReview.IsActive"/> field to false.
    /// </summary>
    [Fact]
    public void Deactivate_WithActiveReview_DeactivatesIt()
    {
        var review = ProductReviewUtils.CreateProductReview();

        review.Deactivate();

        review.IsActive.Should().BeFalse();
    }
}
