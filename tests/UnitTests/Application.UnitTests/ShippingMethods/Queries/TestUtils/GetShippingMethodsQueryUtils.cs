using Application.ShippingMethods.Queries.GetShippingMethods;

namespace Application.UnitTests.ShippingMethods.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetShippingMethodsQuery"/> class.
/// </summary>
public static class GetShippingMethodsQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetShippingMethodsQuery"/> class.
    /// </summary>
    /// <returns>A new instance of the <see cref="GetShippingMethodsQuery"/> class.</returns>
    public static GetShippingMethodsQuery CreateQuery()
    {
        return new GetShippingMethodsQuery();
    }
}
