using Domain.ProductReviewAggregate;

namespace Application.ProductReviews.DTOs.Results;

/// <summary>
/// Represents a product review result.
/// </summary>
public class ProductReviewResult
{
    /// <summary>
    /// Gets the review identifier.
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    public string ProductId { get; }
    /// <summary>
    /// Gets the review title.
    /// </summary>
    public string Title { get; }
    /// <summary>
    /// Gets the review content.
    /// </summary>
    public string Content { get; }
    /// <summary>
    /// Gets the review star rating.
    /// </summary>
    public int StarRating { get; }
    /// <summary>
    /// Gets the date the review was posted.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }
    /// <summary>
    /// Gets the last time the review was updated.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; }

    private ProductReviewResult(ProductReview review)
    {
        Id = review.Id.ToString();
        ProductId = review.ProductId.ToString();
        Title = review.Title;
        Content = review.Content;
        StarRating = review.StarRating.Value;
        CreatedAt = review.CreatedAt;
        UpdatedAt = review.UpdatedAt;
    }

    internal static ProductReviewResult FromReview(ProductReview review)
    {
        return new ProductReviewResult(review);
    }
};
