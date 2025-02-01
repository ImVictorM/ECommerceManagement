using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;
using SharedKernel.UnitTests.TestUtils.Extensions;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Carrier"/> class.
/// </summary>
public static class CarrierUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Carrier"/> class.
    /// </summary>
    /// <param name="id">The carrier id.</param>
    /// <param name="name">The carrier name.</param>
    /// <returns>A new instance of the <see cref="Carrier"/> class.</returns>
    public static Carrier CreateCarrier(
        CarrierId id,
        string? name = null
    )
    {
        var carrier = Carrier.Create(name ?? "ECommerceCarrier");

        if (id != null)
        {
            carrier.SetIdUsingReflection(id);
        }

        return carrier;
    }
}
