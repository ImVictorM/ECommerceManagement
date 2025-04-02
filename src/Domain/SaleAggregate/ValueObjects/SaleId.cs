using SharedKernel.Models;
using SharedKernel.Extensions;

using System.Globalization;

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
    /// <returns>
    /// A new instance of the <see cref="SaleId"/> class with the specified
    /// identifier.
    /// </returns>
    public static SaleId Create(long value)
    {
        return new SaleId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SaleId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="SaleId"/> class with the specified
    /// identifier.
    /// </returns>
    public static SaleId Create(string value)
    {
        return new SaleId(value.ToLongId());
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
