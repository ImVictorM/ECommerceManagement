using Domain.Common.Models;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Entities.Role"/> entity.
/// </summary>
public sealed class RoleId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleId"/> class.
    /// </summary>
    private RoleId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private RoleId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RoleId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="RoleId"/> class with the specified identifier value.</returns>
    public static RoleId Create(long value)
    {
        return new RoleId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
