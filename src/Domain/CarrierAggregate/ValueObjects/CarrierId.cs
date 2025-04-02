using SharedKernel.Extensions;
using SharedKernel.Models;

using System.Globalization;

namespace Domain.CarrierAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for a <see cref="Carrier"/>.
/// </summary>
public sealed class CarrierId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; private set; }

    private CarrierId() { }

    private CarrierId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CarrierId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="CarrierId"/> class with the specified
    /// identifier.
    /// </returns>
    public static CarrierId Create(long value)
    {
        return new CarrierId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CarrierId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="CarrierId"/> class with the specified
    /// identifier.
    /// </returns>
    public static CarrierId Create(string value)
    {
        return new CarrierId(value.ToLongId());
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
