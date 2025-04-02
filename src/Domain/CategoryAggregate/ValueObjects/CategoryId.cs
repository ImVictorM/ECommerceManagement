using SharedKernel.Extensions;
using SharedKernel.Models;

using System.Globalization;

namespace Domain.CategoryAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for a <see cref="Category"/>.
/// </summary>
public class CategoryId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; private set; }

    private CategoryId() { }

    private CategoryId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CategoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="CategoryId"/> class with the specified
    /// identifier.
    /// </returns>
    public static CategoryId Create(long value)
    {
        return new CategoryId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CategoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="CategoryId"/> class with the specified
    /// identifier.
    /// </returns>
    public static CategoryId Create(string value)
    {
        return new CategoryId(value.ToLongId());
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
