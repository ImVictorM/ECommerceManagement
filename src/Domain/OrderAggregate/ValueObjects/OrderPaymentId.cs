using SharedKernel.Models;
using System.Globalization;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for an order payment.
/// </summary>
public class OrderPaymentId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public string Value { get; } = null!;

    /// <summary>
    /// Initiates a new instance of <see cref="OrderPaymentId"/> class.
    /// </summary>
    private OrderPaymentId() { }

    /// <summary>
    /// Initiates a new instance of <see cref="OrderPaymentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private OrderPaymentId(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderPaymentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="OrderPaymentId"/> class with the specified identifier.</returns>
    public static OrderPaymentId Create(string value)
    {
        return new OrderPaymentId(value);
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
