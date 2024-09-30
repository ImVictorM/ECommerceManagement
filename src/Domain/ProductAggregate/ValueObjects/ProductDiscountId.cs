using Domain.Common.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Domain.ProductAggregate.Entities.ProductDiscount"/> entity.
/// </summary>
public sealed class ProductDiscountId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductDiscountId"/> class.
    /// </summary>
    private ProductDiscountId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductDiscountId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ProductDiscountId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductDiscountId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ProductDiscountId"/> class with the specified identifier.</returns>
    public static ProductDiscountId Create(long value)
    {
        return new ProductDiscountId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
