using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;
using Application.Users.Common.DTOs;

namespace Application.Users.Queries.GetAllUsers;

/// <summary>
/// Represents a query to retrieve all all users.
/// </summary>
/// <param name="IsActive">Filter condition for querying the users that is active or not (optional).</param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetAllUsersQuery(bool? IsActive = null) : IRequestWithAuthorization<IEnumerable<UserResult>>
{
    /// <inheritdoc/>
    public string? UserId => null;
}
