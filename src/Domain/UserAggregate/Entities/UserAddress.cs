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
    /// Gets the addresses of the user.
    /// </summary>
    public IEnumerable<AddressId> AddressIds { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserAddress"/> class.
    /// </summary>
    /// <param name="addressIds">The user addresses.</param>
    private UserAddress(IEnumerable<AddressId> addressIds) : base(UserAddressId.Create())
    {
        AddressIds = addressIds;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserAddress"/> class.
    /// </summary>
    /// <param name="addressIds">The user addresses.</param>
    /// <returns>A new instance of the <see cref="UserAddress"/> class.</returns>
    public static UserAddress Create(IEnumerable<AddressId> addressIds)
    {
        return new UserAddress(addressIds);
    }
}
