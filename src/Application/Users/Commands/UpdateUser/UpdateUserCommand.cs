using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;

using MediatR;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
/// Represents a command to update user information.
/// </summary>
/// <param name="UserId">The user to be updated identifier.</param>
/// <param name="Name">The updated name of the user.</param>
/// <param name="Email">The updated email address of the user.</param>
/// <param name="Phone">The updated phone number of the user (optional).</param>
[Authorize(policyType: typeof(RestrictedUpdatePolicy<UpdateUserCommand>))]
public record UpdateUserCommand(
    string UserId,
    string Name,
    string Email,
    string? Phone = null
) : IRequestWithAuthorization<Unit>, IUserSpecificResource;
