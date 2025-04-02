using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;

using MediatR;

namespace Application.ProductReviews.Commands.DeactivateCustomerProductReview;

/// <summary>
/// Represents a command to deactivate a review from a customer.
/// </summary>
/// <param name="UserId">The review owner identifier.</param>
/// <param name="ReviewId">The review identifier.</param>
[Authorize(
    policyType: typeof(SelfOrAdminPolicy<DeactivateCustomerProductReviewCommand>)
)]
public record DeactivateCustomerProductReviewCommand(
    string UserId,
    string ReviewId
) : IUserSpecificResource, IRequestWithAuthorization<Unit>;
