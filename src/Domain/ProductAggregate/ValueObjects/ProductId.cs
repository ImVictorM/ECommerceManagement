using Domain.Common.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Product"/> aggregate root.
/// </summary>
public sealed class ProductId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ProductId(long value)
    {
        Value = value;
    }

    // <summary>
    /// Creates a new instance of the <see cref="ProductId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static ProductId Create()
    {
        return new ProductId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
