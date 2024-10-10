using Domain.Common.Models;

namespace Domain.DiscountAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Discount"/> entity.
/// </summary>
public sealed class DiscountId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="DiscountId"/> class.
    /// </summary>
    private DiscountId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="DiscountId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private DiscountId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DiscountId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="DiscountId"/> class with specified identifier.</returns>
    public static DiscountId Create(long value)
    {
        return new DiscountId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
