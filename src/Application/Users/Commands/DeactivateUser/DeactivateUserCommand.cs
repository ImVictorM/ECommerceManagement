using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;

using MediatR;

namespace Application.Users.Commands.DeactivateUser;

/// <summary>
/// Represents a command to deactivate a user.
/// </summary>
/// <param name="UserId">The user identifier.</param>
[Authorize(policyType: typeof(RestrictedDeactivationPolicy<DeactivateUserCommand>))]
public record DeactivateUserCommand(string UserId)
    : IRequestWithAuthorization<Unit>, IUserSpecificResource;
