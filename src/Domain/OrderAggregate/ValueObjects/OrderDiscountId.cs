using Domain.Common.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Entities.OrderDiscount"/> entity.
/// </summary>
public sealed class OrderDiscountId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; private set; }

    /// <summary>
    /// Initiates a new instance of <see cref="OrderDiscountId"/> class.
    /// </summary>
    private OrderDiscountId() { }

    /// <summary>
    /// Initiates a new instance of <see cref="OrderDiscountId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private OrderDiscountId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderDiscountId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="OrderDiscountId"/> class with the specified identifier.</returns>
    public static OrderDiscountId Create(long value)
    {
        return new OrderDiscountId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
