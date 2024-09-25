using Domain.Common.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Payment"/> aggregate root.
/// </summary>
public sealed class PaymentId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentId"/> class.
    /// </summary>
    private PaymentId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private PaymentId(long value)
    {
        Value = value;
    }

    // <summary>
    /// Creates a new instance of the <see cref="PaymentId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static PaymentId Create()
    {
        return new PaymentId(0);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="PaymentId"/> class with the specified identifier.</returns>
    public static PaymentId Create(long value)
    {
        return new PaymentId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}