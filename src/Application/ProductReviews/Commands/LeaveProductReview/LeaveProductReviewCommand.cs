using Application.Common.Security.Authorization.Requests;
using Application.Common.DTOs.Results;

using SharedKernel.ValueObjects;

namespace Application.ProductReviews.Commands.LeaveProductReview;

/// <summary>
/// Represents a command to leave a review for a product.
/// </summary>
/// <param name="ProductId">The product identifier.</param>
/// <param name="Title">The review title.</param>
/// <param name="Content">The review message content.</param>
/// <param name="StarRating">The product star rating review.</param>
[Authorize(roleName: nameof(Role.Customer))]
public record LeaveProductReviewCommand(
    string ProductId,
    string Title,
    string Content,
    int StarRating
) : IRequestWithAuthorization<CreatedResult>;
