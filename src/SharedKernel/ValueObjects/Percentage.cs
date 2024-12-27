using System.Globalization;
using SharedKernel.Models;

namespace SharedKernel.ValueObjects;

/// <summary>
/// Represents a percentage.
/// </summary>
public class Percentage : ValueObject, IComparable<Percentage>
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
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc/>
    public int CompareTo(Percentage? other)
    {
        return Value.CompareTo(other?.Value);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Percentage other)
        {
            return false;
        }

        return Value.Equals(other.Value);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Indicates whether two <see cref="Percentage"/> instances are equal.
    /// </summary>
    /// <param name="left">The left percentage.</param>
    /// <param name="right">The right percentage.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    public static bool operator ==(Percentage left, Percentage right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Indicates whether two <see cref="Percentage"/> instances are not equal.
    /// </summary>
    /// <param name="left">The left percentage.</param>
    /// <param name="right">The right percentage.</param>
    /// <returns>true if the instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Percentage left, Percentage right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Compares two <see cref="Percentage"/> instances for less than.
    /// </summary>
    /// <param name="left">The left percentage.</param>
    /// <param name="right">The right percentage.</param>
    /// <returns>true if the left instance is less than the right; otherwise, false.</returns>
    public static bool operator <(Percentage left, Percentage right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Compares two <see cref="Percentage"/> instances for less than or equal to.
    /// </summary>
    /// <param name="left">The left percentage.</param>
    /// <param name="right">The right percentage.</param>
    /// <returns>true if the left instance is less than or equal to the right; otherwise, false.</returns>
    public static bool operator <=(Percentage left, Percentage right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Compares two <see cref="Percentage"/> instances for greater than.
    /// </summary>
    /// <param name="left">The left percentage.</param>
    /// <param name="right">The right percentage.</param>
    /// <returns>true if the left instance is greater than the right; otherwise, false.</returns>
    public static bool operator >(Percentage left, Percentage right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Compares two <see cref="Percentage"/> instances for greater than or equal to.
    /// </summary>
    /// <param name="left">The left percentage.</param>
    /// <param name="right">The right percentage.</param>
    /// <returns>true if the left instance is greater than or equal to the right; otherwise, false.</returns>
    public static bool operator >=(Percentage left, Percentage right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
}
