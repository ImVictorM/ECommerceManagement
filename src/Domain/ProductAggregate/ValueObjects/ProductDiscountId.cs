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
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductDiscountId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ProductDiscountId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductDiscountId"/> class with
    /// default identifier value of 0;
    /// </summary>
    /// <returns>A new instance of the <see cref="ProductDiscountId"/> class.</returns>
    public static ProductDiscountId Create()
    {
        return new ProductDiscountId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
