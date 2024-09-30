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
    public long Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallmentId"/> class.
    /// </summary>
    private InstallmentId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallmentId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private InstallmentId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="InstallmentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="InstallmentId"/> class with the specified identifier.</returns>
    public static InstallmentId Create(long value)
    {
        return new InstallmentId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
