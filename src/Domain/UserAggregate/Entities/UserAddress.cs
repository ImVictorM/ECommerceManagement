using Domain.AddressAggregate.ValueObjects;
using Domain.Common.Models;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate.Entities;

/// <summary>
/// Holds user related addresses.
/// </summary>
public sealed class UserAddress : Entity<UserAddressId>
{
    /// <summary>
    /// Gets the user address id.
    /// </summary>
    public AddressId AddressId { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserAddress"/> class.
    /// </summary>
    /// <param name="addressId">The user address id.</param>
    private UserAddress(AddressId addressId) : base(UserAddressId.Create())
    {
        AddressId = addressId;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private UserAddress() : base(UserAddressId.Create()) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// Creates a new instance of the <see cref="UserAddress"/> class.
    /// </summary>
    /// <param name="addressId">The user address id.</param>
    /// <returns>A new instance of the <see cref="UserAddress"/> class.</returns>
    public static UserAddress Create(AddressId addressId)
    {
        return new UserAddress(addressId);
    }
}
