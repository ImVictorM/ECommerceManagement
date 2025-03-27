namespace Contracts.ProductReviews;

/// <summary>
/// Represents a product review user response.
/// </summary>
/// <param name="Id">The user identifier.</param>
/// <param name="Name">The user name.</param>
public record ProductReviewUserResponse(
    string Id,
    string Name
);
