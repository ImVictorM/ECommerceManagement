using Application.Users.Queries.GetUserById;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Users.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetUserByIdQuery"/> class.
/// </summary>
public static class GetUserByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetUserByIdQuery"/> query.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="GetUserByIdQuery"/> query.
    /// </returns>
    public static GetUserByIdQuery CreateQuery(string? id = null)
    {
        return new GetUserByIdQuery(id ?? NumberUtils.CreateRandomLongAsString());
    }
}
