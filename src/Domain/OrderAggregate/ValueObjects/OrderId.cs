using System.Globalization;
using SharedKernel.Errors;
using SharedKernel.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Order"/> aggregate.
/// </summary>
public sealed class OrderId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    private OrderId() { }

    /// <summary>
    /// Initiates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private OrderId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="OrderId"/> class with the specified identifier.</returns>
    public static OrderId Create(long value)
    {
        return new OrderId(value);
    }

    /// <summary>
    /// Creates a new instance of <see cref="OrderId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="OrderId"/> class with the specified identifier.</returns>
    public static OrderId Create(string value)
    {
        if (long.TryParse(value, out var id))
        {
            return new OrderId(id);
        }

        throw new BaseException(
            message: "There was an error when converting the order id.",
            errorCode: ErrorCode.InvalidOperation,
            title: "Domain Error - Invalid Operation"
        );
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
