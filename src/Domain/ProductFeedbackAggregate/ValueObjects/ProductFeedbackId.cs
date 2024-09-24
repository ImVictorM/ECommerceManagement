using Domain.Common.Models;

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

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductFeedbackId"/> class.
    /// </summary>
    private ProductFeedbackId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductFeedbackId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ProductFeedbackId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductFeedbackId"/> class with id placeholder of 0.
    /// </summary>
    /// <returns>A new instance of the <see cref="ProductFeedbackId"/> class.</returns>
    public static ProductFeedbackId Create()
    {
        return new ProductFeedbackId(0);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductFeedbackId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ProductFeedbackId"/> class with the specified identifier.</returns>
    public static ProductFeedbackId Create(long value)
    {
        return new ProductFeedbackId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
