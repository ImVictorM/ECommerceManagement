using Application.ProductReviews.Queries.Projections;

namespace Application.ProductReviews.DTOs.Results;

/// <summary>
/// Represents a detailed product review result.
/// </summary>
public class ProductReviewDetailedResult
{
    /// <summary>
    /// Gets the review identifier.
    /// </summary>
    public string Id { get; }
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
    /// <summary>
    /// Gets the user who posted the review.
    /// </summary>
    public ProductReviewUserResult User { get; }

    private ProductReviewDetailedResult(ProductReviewDetailedProjection projection)
    {
        Id = projection.Id.ToString();
        Title = projection.Title;
        Content = projection.Content;
        StarRating = projection.StarRating.Value;
        CreatedAt = projection.CreatedAt;
        UpdatedAt = projection.UpdatedAt;
        User = ProductReviewUserResult.FromProjection(projection.User);
    }

    internal static ProductReviewDetailedResult FromProjection(
        ProductReviewDetailedProjection projection
    )
    {
        return new ProductReviewDetailedResult(projection);
    }
}
