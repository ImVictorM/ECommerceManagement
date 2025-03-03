namespace Contracts.ProductFeedback;

/// <summary>
/// Represents a product feedback response.
/// </summary>
/// <param name="Id">The product feedback id.</param>
/// <param name="Title">The product feedback title.</param>
/// <param name="Content">The product feedback content.</param>
/// <param name="StarRating">The product feedback star rating.</param>
/// <param name="CreatedAt">The date the product feedback was posted.</param>
/// <param name="UpdatedAt">The date the product feedback was updated.</param>
/// <param name="User">The product feedback user.</param>
public record ProductFeedbackResponse(
    string Id,
    string Title,
    string Content,
    int StarRating,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    ProductFeedbackUserResponse User
);
