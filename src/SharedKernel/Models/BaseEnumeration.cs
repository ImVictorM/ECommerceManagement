using System.Reflection;

namespace SharedKernel.Models;

/// <summary>
/// Base abstraction for enumeration data types.
/// </summary>
public abstract class BaseEnumeration : IComparable
{
    /// <summary>
    /// The item identifier.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// The item name.
    /// </summary>
    public string Name { get; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseEnumeration"/> class.
    /// </summary>
    protected BaseEnumeration() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseEnumeration"/> class.
    /// </summary>
    /// <param name="id">The item identifier.</param>
    /// <param name="name">The item name.</param>
    protected BaseEnumeration(long id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj is not BaseEnumeration enumeration)
        {
            throw new ArgumentException($"Invalid comparison: The provided object is not a valid enumeration of type '{GetType()}'.");
        }

        return Id.CompareTo(enumeration.Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEnumeration other)
        {
            return false;
        }

        var typeMatches = GetType().Equals(other.GetType());
        var idMatches = Id.Equals(other.Id);

        return typeMatches && idMatches;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Retrieves all enumeration values of the specified type.
    /// </summary>
    /// <typeparam name="T">The enumeration type that derives from <see cref="BaseEnumeration"/>.</typeparam>
    /// <returns>An enumerable collection of enumeration values.</returns>
    public static IEnumerable<T> GetAll<T>() where T : BaseEnumeration
    {
        return typeof(T)
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly
            )
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    /// <summary>
    /// Calculates the absolute difference between two enumeration values.
    /// </summary>
    /// <param name="firstValue">The first enumeration value.</param>
    /// <param name="secondValue">The second enumeration value.</param>
    /// <returns>The absolute difference of their identifiers.</returns>
    public static long AbsoluteDifference(BaseEnumeration firstValue, BaseEnumeration secondValue)
    {
        var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
        return absoluteDifference;
    }

    /// <summary>
    /// Retrieves an enumeration value from its identifier.
    /// </summary>
    /// <typeparam name="T">The enumeration type that derives from <see cref="BaseEnumeration"/>.</typeparam>
    /// <param name="value">The identifier of the enumeration value.</param>
    /// <returns>The enumeration value corresponding to the identifier.</returns>
    public static T FromValue<T>(long value)
        where T : BaseEnumeration
    {
        return Parse<T, long>(value, "value", item => item.Id == value);
    }

    /// <summary>
    /// Retrieves an enumeration value from its display name.
    /// </summary>
    /// <typeparam name="T">The enumeration type that derives from <see cref="BaseEnumeration"/>.</typeparam>
    /// <param name="displayName">The display name of the enumeration value.</param>
    /// <returns>The enumeration value corresponding to the display name.</returns>
    public static T FromDisplayName<T>(string displayName)
        where T : BaseEnumeration
    {
        return Parse<T, string>(displayName, "display name", item => item.Name == displayName);
    }

    /// <summary>
    /// Indicates whether two <see cref="BaseEnumeration"/> instances are equal.
    /// </summary>
    /// <param name="left">The left enumeration.</param>
    /// <param name="right">The right enumeration.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    public static bool operator ==(BaseEnumeration left, BaseEnumeration right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Indicates whether two <see cref="BaseEnumeration"/> instances are not equal.
    /// </summary>
    /// <param name="left">The left enumeration.</param>
    /// <param name="right">The right enumeration.</param>
    /// <returns>true if the instances are not equal; otherwise, false.</returns>
    public static bool operator !=(BaseEnumeration left, BaseEnumeration right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Compares two <see cref="BaseEnumeration"/> instances for less than.
    /// </summary>
    /// <param name="left">The left enumeration.</param>
    /// <param name="right">The right enumeration.</param>
    /// <returns>true if the left instance is less than the right; otherwise, false.</returns>
    public static bool operator <(BaseEnumeration left, BaseEnumeration right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Compares two <see cref="BaseEnumeration"/> instances for less than or equal to.
    /// </summary>
    /// <param name="left">The left enumeration.</param>
    /// <param name="right">The right enumeration.</param>
    /// <returns>true if the left instance is less than or equal to the right; otherwise, false.</returns>
    public static bool operator <=(BaseEnumeration left, BaseEnumeration right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Compares two <see cref="BaseEnumeration"/> instances for greater than.
    /// </summary>
    /// <param name="left">The left enumeration.</param>
    /// <param name="right">The right enumeration.</param>
    /// <returns>true if the left instance is greater than the right; otherwise, false.</returns>
    public static bool operator >(BaseEnumeration left, BaseEnumeration right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Compares two <see cref="BaseEnumeration"/> instances for greater than or equal to.
    /// </summary>
    /// <param name="left">The left enumeration.</param>
    /// <param name="right">The right enumeration.</param>
    /// <returns>true if the left instance is greater than or equal to the right; otherwise, false.</returns>
    public static bool operator >=(BaseEnumeration left, BaseEnumeration right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Parses the provided value to find a corresponding enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type that derives from <see cref="BaseEnumeration"/>.</typeparam>
    /// <typeparam name="K">The type of the value to parse.</typeparam>
    /// <param name="value">The value to be parsed.</param>
    /// <param name="description">A description of the value for error messaging.</param>
    /// <param name="predicate">A predicate to evaluate for matching the enumeration.</param>
    /// <returns>The matching enumeration value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching enumeration value is found.</exception>
    private static T Parse<T, K>(K value, string description, Func<T, bool> predicate)
        where T : BaseEnumeration
    {
        return
            GetAll<T>().FirstOrDefault(predicate) ??
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
    }
}
