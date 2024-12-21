using SharedKernel.Models;

namespace SharedKernel.ValueObjects;

/// <summary>
/// Represents a percentage.
/// </summary>
public class Percentage : ValueObject
{
    /// <summary>
    /// Gets the percentage value.
    /// </summary>
    public int Value { get; }

    private Percentage() { }

    private Percentage(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new <see cref="Percentage"/> instance.
    /// </summary>
    /// <param name="value">The percentage value.</param>
    /// <returns>A valid <see cref="Percentage"/> object.</returns>
    public static Percentage Create(int value)
    {
        return new Percentage(value);
    }

    /// <summary>
    /// Determines whether the percentage value falls within the specified range (inclusive).
    /// </summary>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <returns>True if the percentage is within the range; otherwise, false.</returns>
    public bool IsWithinRange(int start, int end)
    {
        return Value >= start && Value <= end;
    }

    /// <summary>
    /// Determines whether the percentage value falls within the range of 0 to the specified end value (inclusive).
    /// </summary>
    /// <param name="end">The end of the range.</param>
    /// <returns>True if the percentage is between 0 and the end value; otherwise, false.</returns>
    public bool IsWithinRange(int end)
    {
        return IsWithinRange(0, end);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
