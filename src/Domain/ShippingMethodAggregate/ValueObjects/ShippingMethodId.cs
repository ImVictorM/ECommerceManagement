using SharedKernel.Extensions;
using SharedKernel.Models;

using System.Globalization;

namespace Domain.ShippingMethodAggregate.ValueObjects;

/// <summary>
/// Represents an identifier of a <see cref="ShippingMethod"/> aggregate root.
/// </summary>
public sealed class ShippingMethodId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    private ShippingMethodId() { }

    private ShippingMethodId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShippingMethodId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ShippingMethodId"/> class with the specified identifier.</returns>
    public static ShippingMethodId Create(long value)
    {
        return new ShippingMethodId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShippingMethodId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ShippingMethodId"/> class with the specified identifier.</returns>
    public static ShippingMethodId Create(string value)
    {
        return new ShippingMethodId(value.ToLongId());
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
