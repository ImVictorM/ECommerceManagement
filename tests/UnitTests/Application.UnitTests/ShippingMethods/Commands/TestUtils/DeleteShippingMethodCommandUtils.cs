using Application.ShippingMethods.Commands.DeleteShippingMethod;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.ShippingMethods.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="DeleteShippingMethodCommand"/> class.
/// </summary>
public static class DeleteShippingMethodCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeleteShippingMethodCommand"/>
    /// class.
    /// </summary>
    /// <param name="shippingMethodId">The shipping method identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="DeleteShippingMethodCommand"/> class.
    /// </returns>
    public static DeleteShippingMethodCommand CreateCommand(
        string? shippingMethodId = null
    )
    {
        return new DeleteShippingMethodCommand(
            shippingMethodId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
