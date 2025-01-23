using Application.Users.Queries.GetSelf;

namespace Application.UnitTests.Users.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetSelfQuery"/> class.
/// </summary>
public static class GetSelfQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetSelfQuery"/> class.
    /// </summary>
    /// <returns>A new instance of the <see cref="GetSelfQuery"/> class.</returns>
    public static GetSelfQuery CreateQuery()
    {
        return new GetSelfQuery();
    }
}
