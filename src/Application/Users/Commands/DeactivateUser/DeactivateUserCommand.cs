using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;

using MediatR;

namespace Application.Users.Commands.DeactivateUser;

/// <summary>
/// Represents a command to deactivate and user.
/// </summary>
/// <param name="UserId">The id of the user to be deactivated.</param>
[Authorize(policyType: typeof(RestrictedDeactivationPolicy))]
public record DeactivateUserCommand(string UserId) : IRequestWithAuthorization<Unit>;
