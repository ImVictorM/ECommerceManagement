using SharedKernel.Extensions;
using SharedKernel.Models;

using System.Globalization;

namespace Domain.ProductFeedbackAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="ProductFeedback"/> aggregate root.
/// </summary>
public sealed class ProductFeedbackId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    private ProductFeedbackId() { }

    private ProductFeedbackId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductFeedbackId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductFeedbackId"/> class
    /// with the specified identifier.
    /// </returns>
    public static ProductFeedbackId Create(long value)
    {
        return new ProductFeedbackId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductFeedbackId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductFeedbackId"/> class
    /// with the specified identifier.
    /// </returns>
    public static ProductFeedbackId Create(string value)
    {
        return new ProductFeedbackId(value.ToLongId());
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
