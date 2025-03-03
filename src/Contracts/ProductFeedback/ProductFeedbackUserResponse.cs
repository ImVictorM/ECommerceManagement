namespace Contracts.ProductFeedback;

/// <summary>
/// Represents a product feedback user response.
/// </summary>
/// <param name="Id">The user id.</param>
/// <param name="Name">The user name.</param>
public record ProductFeedbackUserResponse(
    string Id,
    string Name
);
