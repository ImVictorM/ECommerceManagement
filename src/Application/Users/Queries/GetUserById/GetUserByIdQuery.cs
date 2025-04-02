using Application.Common.Security.Authorization.Requests;
using Application.Users.DTOs.Results;

using SharedKernel.ValueObjects;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// Represents a query to retrieve a user by their identifier.
/// </summary>
/// <param name="UserId">The user identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetUserByIdQuery(string UserId)
    : IRequestWithAuthorization<UserResult>, IUserSpecificResource;
