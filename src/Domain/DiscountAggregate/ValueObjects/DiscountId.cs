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
    /// <param name="value">The identifier value.</param>
    private DiscountId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DiscountId"/> class.
    /// </summary>
    /// <returns>A new instance of the <see cref="DiscountId"/> class.</returns>
    public static DiscountId Create()
    {
        return new DiscountId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
