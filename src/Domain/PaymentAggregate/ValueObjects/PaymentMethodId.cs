using Domain.Common.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Entities.PaymentMethod"/> entity.
/// </summary>
public sealed class PaymentMethodId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentMethodId"/> class.
    /// </summary>
    private PaymentMethodId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentMethodId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private PaymentMethodId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentMethodId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="PaymentMethodId"/> class with the specified identifier.</returns>
    public static PaymentMethodId Create(long value)
    {
        return new PaymentMethodId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
