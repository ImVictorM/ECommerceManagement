namespace Contracts.ProductFeedback;

/// <summary>
/// Represents a request to leave product feedback.
/// </summary>
/// <param name="Title">The feedback title.</param>
/// <param name="Content">The feedback content.</param>
/// <param name="StarRating">The feedback star rating.</param>
public record LeaveProductFeedbackRequest(
    string Title,
    string Content,
    int StarRating
);
