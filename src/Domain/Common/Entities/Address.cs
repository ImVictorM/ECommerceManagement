using Domain.Common.Models;
using Domain.Common.ValueObjects;

namespace Domain.Common.Entities;

/// <summary>
/// Represents an address.
/// </summary>
public sealed class Address : Entity<AddressId>
{
    /// <summary>
    /// Gets the address postal code.
    /// </summary>
    public string PostalCode { get; private set; }
    /// <summary>
    /// Gets the address street.
    /// </summary>
    public string Street { get; private set; }
    /// <summary>
    /// Gets the address neighborhood.
    /// </summary>
    public string Neighborhood { get; private set; }
    /// <summary>
    /// Gets the address state.
    /// </summary>
    public string State { get; private set; }
    /// <summary>
    /// Gets the address city.
    /// </summary>
    public string City { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="Address"/> class.
    /// </summary>
    /// <param name="postalCode">The address postal code.</param>
    /// <param name="street">The address street.</param>
    /// <param name="neighborhood">The address neighborhood.</param>
    /// <param name="state">The address state.</param>
    /// <param name="city">The address city.</param>
    private Address(
        string postalCode,
        string street,
        string neighborhood,
        string state,
        string city
    ) : base(AddressId.Create())
    {
        PostalCode = postalCode;
        Street = street;
        Neighborhood = neighborhood;
        State = state;
        City = city;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Address"/> class.
    /// </summary>
    /// <param name="postalCode">The address postal code.</param>
    /// <param name="street">The address street.</param>
    /// <param name="neighborhood">The address neighborhood.</param>
    /// <param name="state">The address state.</param>
    /// <param name="city">The address city.</param>
    /// <returns>A new instance of the <see cref="Address"/> class.</returns>
    public static Address Create(
        string postalCode,
        string street,
        string neighborhood,
        string state,
        string city
    )
    {
        return new Address(
            postalCode,
            street,
            neighborhood,
            state,
            city
        );
    }
}
