using System.Globalization;
using SharedKernel.Errors;
using SharedKernel.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Product"/> aggregate root.
/// </summary>
public sealed class ProductId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductId"/> class.
    /// </summary>
    private ProductId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ProductId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ProductId"/> class with the specified identifier.</returns>
    public static ProductId Create(long value)
    {
        return new ProductId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ProductId"/> class with the specified identifier.</returns>
    public static ProductId Create(string value)
    {
        if (long.TryParse(value, out var id))
        {
            return new ProductId(id);
        }

        throw new BaseException(
            message: "There was an error when converting a product id.",
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
