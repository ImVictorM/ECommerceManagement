using Domain.Common.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Entities.PaymentStatusHistory"/> entity.
/// </summary>
public sealed class PaymentStatusHistoryId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentStatusHistoryId"/> class.
    /// </summary>
    private PaymentStatusHistoryId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentStatusHistoryId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private PaymentStatusHistoryId(long value)
    {
        Value = value;
    }

    /// <summary>
    ///  Creates a new instance of the <see cref="PaymentStatusHistoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="PaymentStatusHistoryId"/> class with the specified identifier.</returns>
    public static PaymentStatusHistoryId Create(long value)
    {
        return new PaymentStatusHistoryId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
