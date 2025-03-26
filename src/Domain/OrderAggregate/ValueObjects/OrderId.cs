using SharedKernel.Extensions;
using SharedKernel.Models;

using System.Globalization;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Order"/> aggregate.
/// </summary>
public sealed class OrderId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    private OrderId() { }

    private OrderId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of <see cref="OrderId"/> class with the specified
    /// identifier.
    /// </returns>
    public static OrderId Create(long value)
    {
        return new OrderId(value);
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of <see cref="OrderId"/> class with the specified
    /// identifier.
    /// </returns>
    public static OrderId Create(string value)
    {
        return new OrderId(value.ToLongId());
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
