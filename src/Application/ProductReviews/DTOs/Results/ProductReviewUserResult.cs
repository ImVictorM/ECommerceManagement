using Application.ProductReviews.Queries.Projections;

namespace Application.ProductReviews.DTOs.Results;

/// <summary>
/// Represents a product review user result.
/// </summary>
public class ProductReviewUserResult
{
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string Name { get; }

    private ProductReviewUserResult(ProductReviewUserProjection projection)
    {
        Id = projection.Id.ToString();
        Name = projection.Name;
    }

    internal static ProductReviewUserResult FromProjection(
        ProductReviewUserProjection projection
    )
    {
        return new ProductReviewUserResult(projection);
    }
}
