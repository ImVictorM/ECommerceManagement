using Domain.Common.Models;

namespace Domain.RoleAggregate.ValueObjects;

/// <summary>
/// Identifier for the <see cref="Role"/> aggregate root.
/// </summary>
public sealed class RoleId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleId"/> class.
    /// </summary>
    private RoleId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleId"/> class with the specified identifier value.
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
    /// <returns>A new instance of the <see cref="RoleId"/> class with the specified identifier.</returns>
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
