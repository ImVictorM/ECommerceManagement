using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Carrier"/> class.
/// </summary>
public static class CarrierUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Carrier"/> class.
    /// </summary>
    /// <param name="id">The carrier identifier.</param>
    /// <param name="name">The carrier name.</param>
    /// <param name="email">The carrier email.</param>
    /// <param name="passwordHash">The carrier password hash.</param>
    /// <param name="phone">The carrier phone.</param>
    /// <returns>A new instance of the <see cref="Carrier"/> class.</returns>
    public static Carrier CreateCarrier(
        CarrierId? id = null,
        string? name = null,
        Email? email = null,
        PasswordHash? passwordHash = null,
        string? phone = null
    )
    {
        var carrier = Carrier.Create(
            name ?? "ECommerceCarrier",
            email ?? EmailUtils.CreateEmail(),
            passwordHash ?? PasswordHashUtils.Create(),
            phone
        );

        if (id != null)
        {
            carrier.SetIdUsingReflection(id);
        }

        return carrier;
    }
}
