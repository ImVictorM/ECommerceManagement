using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;

using MediatR;

namespace Application.ProductFeedback.Commands.DeactivateCustomerProductFeedback;

/// <summary>
/// Represents a command to deactivate customer product feedback.
/// </summary>
/// <param name="UserId">The feedback owner identifier.</param>
/// <param name="FeedbackId">The feedback to be deactivate identifier.</param>
[Authorize(
    policyType: typeof(SelfOrAdminPolicy<DeactivateCustomerProductFeedbackCommand>)
)]
public record DeactivateCustomerProductFeedbackCommand(
    string UserId,
    string FeedbackId
) : IUserSpecificResource, IRequestWithAuthorization<Unit>;
