using Domain.Common.Models;
using Domain.Common.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate.Entities;

/// <summary>
/// Holds user related addresses.
/// </summary>
public sealed class UserAddress : Entity<UserAddressId>
{
    /// <summary>
    /// Gets the user address.
    /// </summary>
    public Address Address { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="UserAddress"/> class.
    /// </summary>
    private UserAddress() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserAddress"/> class.
    /// </summary>
    /// <param name="address">The user address.</param>
    private UserAddress(Address address)
    {
        Address = address;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserAddress"/> class.
    /// </summary>
    /// <param name="address">The user address.</param>
    /// <returns>A new instance of the <see cref="UserAddress"/> class.</returns>
    public static UserAddress Create(Address address)
    {
        return new UserAddress(address);
    }
}
