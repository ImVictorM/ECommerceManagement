using SharedKernel.Extensions;
using SharedKernel.Models;

using System.Globalization;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Product"/> aggregate root.
/// </summary>
public sealed class ProductId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; private set; }

    private ProductId() { }

    private ProductId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductId"/> class with the specified
    /// identifier.
    /// </returns>
    public static ProductId Create(long value)
    {
        return new ProductId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductId"/> class with the specified
    /// identifier.
    /// </returns>
    public static ProductId Create(string value)
    {
        return new ProductId(value.ToLongId());
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
