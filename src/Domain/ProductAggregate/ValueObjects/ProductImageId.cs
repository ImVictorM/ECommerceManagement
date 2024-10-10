using Domain.Common.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Domain.ProductAggregate.Entities.ProductImage"/> entity.
/// </summary>
public sealed class ProductImageId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductImageId"/> class.
    /// </summary>
    private ProductImageId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductImageId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ProductImageId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductImageId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ProductImageId"/> class with the specified identifier.</returns>
    public static ProductImageId Create(long value)
    {
        return new ProductImageId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
