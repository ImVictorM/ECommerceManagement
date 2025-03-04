using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;

namespace Domain.UnitTests.ProductFeedbackAggregate;

/// <summary>
/// Unit tests for the <see cref="ProductFeedback"/> aggregate root.
/// </summary>
public class ProductFeedbackTests
{
    /// <summary>
    /// List containing valid parameters to create a new product feedback.
    /// </summary>
    public static readonly IEnumerable<object[]> CreateProductFeedbackValidParameters =
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
    /// Verifies it is possible to create a product feedback with valid parameters.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="productId">The product id.</param>
    /// <param name="title">The feedback title.</param>
    /// <param name="content">The feedback content.</param>
    /// <param name="starRating">The feedback star rating.</param>
    [Theory]
    [MemberData(nameof(CreateProductFeedbackValidParameters))]
    public void CreateProductFeedback_WithValidParameters_ReturnsInstance(
        UserId userId,
        ProductId productId,
        string title,
        string content,
        StarRating starRating
    )
    {
        var action = FluentActions
            .Invoking(() => ProductFeedback.Create(
                userId: userId,
                productId: productId,
                title: title,
                content: content,
                starRating: starRating
            ))
            .Should()
            .NotThrow();

        var feedback = action.Subject;

        feedback.UserId.Should().Be(userId);
        feedback.ProductId.Should().Be(productId);
        feedback.Title.Should().Be(title);
        feedback.Content.Should().Be(content);
        feedback.StarRating.Should().Be(starRating);
        feedback.IsActive.Should().BeTrue();
    }

    /// <summary>
    /// Verifies deactivating the product feedback makes the feedback inactive
    /// by setting the <see cref="ProductFeedback.IsActive"/> field to false.
    /// </summary>
    [Fact]
    public void DeactivateProductFeedback_WhenCalled_MakesTheFeedbackInactive()
    {
        var feedback = ProductFeedbackUtils.CreateProductFeedback();

        feedback.Deactivate();

        feedback.IsActive.Should().BeFalse();
    }
}
