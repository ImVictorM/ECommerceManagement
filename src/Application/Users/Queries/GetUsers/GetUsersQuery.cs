using Application.Common.Security.Authorization.Requests;
using Application.Users.DTOs.Filters;
using Application.Users.DTOs.Results;

using SharedKernel.ValueObjects;

namespace Application.Users.Queries.GetUsers;

/// <summary>
/// Represents a query to retrieve all the users.
/// </summary>
/// <param name="Filters">
/// The filters to apply in the query.
/// </param>
[Authorize(roleName: nameof(Role.Admin))]
public record GetUsersQuery(UserFilters Filters)
    : IRequestWithAuthorization<IReadOnlyList<UserResult>>;
