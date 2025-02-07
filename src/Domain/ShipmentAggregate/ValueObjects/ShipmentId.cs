using SharedKernel.Extensions;
using SharedKernel.Models;

using System.Globalization;

namespace Domain.ShipmentAggregate.ValueObjects;

/// <summary>
/// Represents an identifier of a <see cref="Shipment"/> aggregate root.
/// </summary>
public sealed class ShipmentId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    private ShipmentId() { }

    private ShipmentId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ShipmentId"/> class with the specified identifier.</returns>
    public static ShipmentId Create(long value)
    {
        return new ShipmentId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ShipmentId"/> class with the specified value.</returns>
    public static ShipmentId Create(string value)
    {
        return new ShipmentId(value.ToLongId());
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
