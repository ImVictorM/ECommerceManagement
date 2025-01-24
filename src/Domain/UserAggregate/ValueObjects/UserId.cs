using System.Globalization;
using SharedKernel.Extensions;
using SharedKernel.Models;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="User"/> aggregate root.
/// </summary>
public sealed class UserId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    private UserId() { }

    private UserId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="UserId"/> class with the specified value.</returns>
    public static UserId Create(long value)
    {
        return new UserId(value);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="UserId"/> class with the specified value.</returns>
    public static UserId Create(string value)
    {
        return new UserId(value.ToLongId());
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
