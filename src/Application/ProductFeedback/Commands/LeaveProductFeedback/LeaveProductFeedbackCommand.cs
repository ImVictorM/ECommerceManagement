using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;
using Application.Common.DTOs.Results;

namespace Application.ProductFeedback.Commands.LeaveProductFeedback;

/// <summary>
/// Represents a command to leave a product feedback.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="Title">The feedback title.</param>
/// <param name="Content">The feedback message content.</param>
/// <param name="StarRating">The product star rating feedback.</param>
[Authorize(roleName: nameof(Role.Customer))]
public record LeaveProductFeedbackCommand(
    string ProductId,
    string Title,
    string Content,
    int StarRating
) : IRequestWithAuthorization<CreatedResult>;
