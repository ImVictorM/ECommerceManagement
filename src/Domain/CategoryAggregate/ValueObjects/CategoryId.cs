using SharedKernel.Errors;
using SharedKernel.Models;
using System.Globalization;

namespace Domain.CategoryAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Category"/> aggregate root.
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
    /// <returns>A new instance of the <see cref="CategoryId"/> class with the specified identifier.</returns>
    public static CategoryId Create(long value)
    {
        return new CategoryId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CategoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="CategoryId"/> class with the specified identifier.</returns>
    public static CategoryId Create(string value)
    {
        if (long.TryParse(value, out var id))
        {
            return new CategoryId(id);
        }

        throw new BaseException(
            message: "There was an error when converting a category id.",
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
