using SharedKernel.Models;
using System.Globalization;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for a payment.
/// </summary>
public class PaymentId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public string Value { get; } = null!;

    /// <summary>
    /// Initiates a new instance of <see cref="PaymentId"/> class.
    /// </summary>
    private PaymentId() { }

    /// <summary>
    /// Initiates a new instance of <see cref="PaymentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private PaymentId(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="PaymentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="PaymentId"/> class with the specified identifier.</returns>
    public static PaymentId Create(string value)
    {
        return new PaymentId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
