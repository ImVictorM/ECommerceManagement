using Domain.Common.Models;

namespace Domain.ProductCategoryAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for a <see cref="ProductCategory"/> aggregate.
/// </summary>
public sealed class ProductCategoryId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategoryId"/> class.
    /// </summary>
    private ProductCategoryId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategoryId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ProductCategoryId(long value)
    {
        Value = value;
    }

    // <summary>
    /// Creates a new instance of the <see cref="ProductCategoryId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static ProductCategoryId Create()
    {
        return new ProductCategoryId(0);
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
