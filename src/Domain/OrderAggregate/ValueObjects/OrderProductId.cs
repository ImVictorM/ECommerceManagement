using Domain.Common.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents the idenfier for the <see cref="Domain.OrderAggregate.Entities.OrderProduct"/> entity.
/// </summary>
public sealed class OrderProductId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderProductId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private OrderProductId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderProductId"/> class with default identifier
    /// placeholder value of 0.
    /// </summary>
    /// <returns>A new instance of the <see cref="OrderProductId"/> class.</returns>
    public static OrderProductId Create()
    {
        return new OrderProductId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
