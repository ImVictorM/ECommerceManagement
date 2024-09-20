using Domain.Common.Models;

namespace Domain.InstallmentAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Installment"/> aggregate root.
/// </summary>
public sealed class InstallmentId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallmentId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private InstallmentId(long value)
    {
        Value = value;
    }

    // <summary>
    /// Creates a new instance of the <see cref="InstallmentId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static InstallmentId Create()
    {
        return new InstallmentId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
