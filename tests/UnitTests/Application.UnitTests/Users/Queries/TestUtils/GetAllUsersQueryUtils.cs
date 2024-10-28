using Application.Users.Queries.GetAllUsers;

namespace Application.UnitTests.Users.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetAllUsersQuery"/> query.
/// </summary>
public static class GetAllUsersQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetAllUsersQuery"/> query.
    /// </summary>
    /// <param name="isActive">Optional. Filter condition to query users that are active or not.</param>
    /// <returns>A new instance of the <see cref="GetAllUsersQuery"/> query.</returns>
    public static GetAllUsersQuery CreateQuery(bool? isActive = null)
    {
        return new GetAllUsersQuery(isActive);
    }
}
