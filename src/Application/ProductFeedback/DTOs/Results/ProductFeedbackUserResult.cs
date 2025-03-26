using Domain.UserAggregate.ValueObjects;

namespace Application.ProductFeedback.DTOs.Results;

/// <summary>
/// Represents a product feedback user result.
/// </summary>
/// <param name="Id">The user id.</param>
/// <param name="Name">The user name.</param>
public record ProductFeedbackUserResult(
    UserId Id,
    string Name
);
