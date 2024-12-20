using SharedKernel.Errors;
using System.Globalization;
using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Sale"/> aggregate root.
/// </summary>
public class SaleId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; private set; }

    private SaleId() { }

    private SaleId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SaleId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="SaleId"/> class with the specified identifier.</returns>
    public static SaleId Create(long value)
    {
        return new SaleId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SaleId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="SaleId"/> class with the specified identifier.</returns>
    public static SaleId Create(string value)
    {
        if (long.TryParse(value, out var id))
        {
            return new SaleId(id);
        }

        throw new BaseException(
            message: "There was an error when converting a sale id.",
            errorCode: ErrorCode.InvalidOperation,
            title: "Domain Error - Invalid Operation"
        );
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
