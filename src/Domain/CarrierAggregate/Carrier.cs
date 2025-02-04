using Domain.CarrierAggregate.ValueObjects;

using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.CarrierAggregate;

/// <summary>
/// Represents a shipment carrier.
/// </summary>
public sealed class Carrier : AggregateRoot<CarrierId>
{
    /// <summary>
    /// Gets the carrier name.
    /// </summary>
    public string Name { get; private set; } = null!;
    /// <summary>
    /// Gets the carrier email.
    /// </summary>
    public Email Email { get; private set; } = null!;
    /// <summary>
    /// Gets the carrier password hash.
    /// </summary>
    public PasswordHash PasswordHash { get; private set; } = null!;
    /// <summary>
    /// Gets the carrier phone.
    /// </summary>
    public string? Phone { get; private set; }

    private Carrier() { }

    private Carrier(string name, Email email, PasswordHash passwordHash, string? phone = null)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Phone = phone;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Carrier"/> class.
    /// </summary>
    /// <param name="name">The carrier name.</param>
    /// <param name="email">The carrier email.</param>
    /// <param name="passwordHash">The carrier password hash.</param>
    /// <param name="phone">The carrier phone.</param>
    /// <returns>A new instance of the <see cref="Carrier"/> class.</returns>
    public static Carrier Create(string name, Email email, PasswordHash passwordHash, string? phone = null)
    {
        return new Carrier(name, email, passwordHash, phone);
    }
}
