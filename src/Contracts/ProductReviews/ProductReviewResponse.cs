namespace Contracts.ProductReviews;

/// <summary>
/// Represents a product review response.
/// </summary>
/// <param name="Id">The product review identifier.</param>
/// <param name="ProductId">The product identifier.</param>
/// <param name="Title">The product review title.</param>
/// <param name="Content">The product review content.</param>
/// <param name="StarRating">The product review star rating.</param>
/// <param name="CreatedAt">The date the product review was posted.</param>
/// <param name="UpdatedAt">The date the product review was last updated.</param>
public record ProductReviewResponse(
    string Id,
    string ProductId,
    string Title,
    string Content,
    int StarRating,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
