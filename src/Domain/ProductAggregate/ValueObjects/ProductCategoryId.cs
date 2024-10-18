using Domain.Common.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Entities.ProductCategory"/> entity.
/// </summary>
public sealed class ProductCategoryId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductCategoryId"/> class.
    /// </summary>
    private ProductCategoryId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductCategoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ProductCategoryId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductCategoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ProductCategoryId"/> class with the specified identifier.</returns>
    public static ProductCategoryId Create(long value)
    {
        return new ProductCategoryId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
