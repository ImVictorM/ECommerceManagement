using SharedKernel.Extensions;
using SharedKernel.Models;

using System.Globalization;

namespace Domain.ProductReviewAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for <see cref="ProductReview"/>.
/// </summary>
public sealed class ProductReviewId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    private ProductReviewId() { }

    private ProductReviewId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductReviewId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductReviewId"/> class
    /// with the specified identifier.
    /// </returns>
    public static ProductReviewId Create(long value)
    {
        return new ProductReviewId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductReviewId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductReviewId"/> class
    /// with the specified identifier.
    /// </returns>
    public static ProductReviewId Create(string value)
    {
        return new ProductReviewId(value.ToLongId());
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
