using Domain.ProductAggregate.ValueObjects;
using Domain.ProductReviewAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Domain.ProductReviewAggregate;

/// <summary>
/// Represents a product review.
/// </summary>
public sealed class ProductReview : AggregateRoot<ProductReviewId>, IActivatable
{
    /// <summary>
    /// Gets the product review title.
    /// </summary>
    public string Title { get; private set; } = null!;
    /// <summary>
    /// Gets the product review content.
    /// </summary>
    public string Content { get; private set; } = null!;
    /// <summary>
    /// Gets the product star rating.
    /// </summary>
    public StarRating StarRating { get; private set; } = null!;
    /// <summary>
    /// A boolean indicating if the review is active.
    /// </summary>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the identifier of the product the review references to.
    /// </summary>
    public ProductId ProductId { get; private set; } = null!;
    /// <summary>
    /// Gets the identifier of the user who created the review.
    /// </summary>
    public UserId UserId { get; private set; } = null!;

    private ProductReview() { }

    private ProductReview(
        UserId userId,
        ProductId productId,
        string title,
        string content,
        StarRating starRating
    )
    {
        Title = title;
        Content = content;
        StarRating = starRating;
        ProductId = productId;
        UserId = userId;

        IsActive = true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductReview"/> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="productId">The product identifier.</param>
    /// <param name="title">The review title.</param>
    /// <param name="content">The review text content.</param>
    /// <param name="starRating">The review star rating.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductReview"/> class.
    /// </returns>
    public static ProductReview Create(
        UserId userId,
        ProductId productId,
        string title,
        string content,
        StarRating starRating
    )
    {
        return new ProductReview(
            userId,
            productId,
            title,
            content,
            starRating
        );
    }

    /// <inheritdoc/>
    public void Deactivate()
    {
        IsActive = false;
    }
}
