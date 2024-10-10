namespace Domain.Common.Models;

/// <summary>
/// Base class for domain value objects.
/// Value objects are immutable types that are defined by their properties
/// rather than their identity.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Define the equality components for the value object.
    /// Indicate the properties used for equality comparison.
    /// </summary>
    /// <returns>An enumerable of the properties used to define equality.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;

        var valueObject = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    /// <inheritdoc/>
    public virtual bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }

    /// <summary>
    /// Equality operator for value objects.
    /// </summary>
    /// <param name="left">left value object to compare.</param>
    /// <param name="right">right value object to compare.</param>
    /// <returns>A boolean indicating the equality of the value objects.</returns>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Inequality operator for value objects.
    /// </summary>
    /// <param name="left">left value object to compare.</param>
    /// <param name="right">right value object to compare.</param>
    /// <returns>A boolean indicating the inequality of the value objects.</returns>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select((property) => property?.GetHashCode() ?? 0)
            .Aggregate((acc, curr) => acc ^ curr);
    }
}
