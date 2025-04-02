using Application.Users.DTOs.Filters;
using Application.Users.Queries.GetUsers;

namespace Application.UnitTests.Users.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetUsersQuery"/> query.
/// </summary>
public static class GetUsersQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetUsersQuery"/> query.
    /// </summary>
    /// <param name="filters">The user filters.</param>
    /// <returns>
    /// A new instance of the <see cref="GetUsersQuery"/> query.
    /// </returns>
    public static GetUsersQuery CreateQuery(UserFilters? filters = null)
    {
        return new GetUsersQuery(
            filters ?? new()
        );
    }
}
