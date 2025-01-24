using SharedKernel.Models;

namespace SharedKernel.ValueObjects;

/// <summary>
/// Represents an address.
/// </summary>
public sealed class Address : ValueObject
{
    /// <summary>
    /// Gets the address postal code.
    /// </summary>
    public string PostalCode { get; } = string.Empty;
    /// <summary>
    /// Gets the address street.
    /// </summary>
    public string Street { get; } = string.Empty;
    /// <summary>
    /// Gets the address neighborhood.
    /// </summary>
    public string? Neighborhood { get; }
    /// <summary>
    /// Gets the address state.
    /// </summary>
    public string State { get; } = string.Empty;
    /// <summary>
    /// Gets the address city.
    /// </summary>
    public string City { get; } = string.Empty;

    private Address() { }

    private Address(
        string postalCode,
        string street,
        string state,
        string city,
        string? neighborhood = null
    )
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
        string state,
        string city,
        string? neighborhood = null
    )
    {
        return new Address(
            postalCode,
            street,
            state,
            city,
            neighborhood
        );
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PostalCode;
        yield return Street;
        yield return State;
        yield return City;
        yield return Neighborhood;
    }
}

