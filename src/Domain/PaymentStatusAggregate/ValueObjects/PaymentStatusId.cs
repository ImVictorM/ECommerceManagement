using Domain.Common.Models;

namespace Domain.PaymentStatusAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="PaymentStatus"/> entity.
/// </summary>
public sealed class PaymentStatusId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentStatusId"/> class.
    /// </summary>
    private PaymentStatusId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentStatusId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private PaymentStatusId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatusId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="PaymentStatusId"/> class with the specified identifier.</returns>
    public static PaymentStatusId Create(long value)
    {
        return new PaymentStatusId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
