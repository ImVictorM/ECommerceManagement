using Application.ShippingMethods.Queries.GetShippingMethodById;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.ShippingMethods.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetShippingMethodByIdQuery"/> class.
/// </summary>
public static class GetShippingMethodByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetShippingMethodByIdQuery"/>
    /// class.
    /// </summary>
    /// <param name="shippingMethodId">The shipping method identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="GetShippingMethodByIdQuery"/> class.
    /// </returns>
    public static GetShippingMethodByIdQuery CreateQuery(
        string? shippingMethodId = null
    )
    {
        return new GetShippingMethodByIdQuery(
            shippingMethodId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
