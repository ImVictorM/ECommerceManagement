using Application.Users.Queries.GetUserById;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Users.Queries.TestUtils;

/// <summary>
/// Utilities for the get user by id query.
/// </summary>
public static class GetUserByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetUserByIdQuery"/> query.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns>A new instance of the <see cref="GetUserByIdQuery"/> query. </returns>
    public static GetUserByIdQuery CreateQuery(string? id = null)
    {
        return new GetUserByIdQuery(id ?? DomainConstants.User.Id.ToString());
    }
}
