using Domain.Common.Models;

namespace Domain.Users.ValueObjects;

/// <summary>
/// Identifier for the <see cref="Domain.Users.Entities.Role"/> entity.
/// </summary>
public sealed class RoleId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private RoleId(long value)
    {
        Value = value;
    }

    // <summary>
    /// Creates a new instance of the <see cref="RoleId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static RoleId Create()
    {
        return new RoleId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
