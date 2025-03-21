using Application.Common.Security.Authorization.Requests;
using Application.Users.DTOs;

using SharedKernel.ValueObjects;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// Query to get the user by identifier.
/// </summary>
/// <param name="UserId">The user identifier value.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetUserByIdQuery(string UserId)
    : IRequestWithAuthorization<UserResult>, IUserSpecificResource;
