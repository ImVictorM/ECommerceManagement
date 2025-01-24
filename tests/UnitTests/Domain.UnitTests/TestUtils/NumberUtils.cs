using System.Globalization;
using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities related to numbers.
/// </summary>
public static class NumberUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new list of ascending sequential numbers starting in 1.
    /// </summary>
    /// <param name="count">The quantity of numbers to be generated.</param>
    /// <returns>A list containing the generated numbers.</returns>
    public static IEnumerable<string> CreateNumberSequenceAsString(int count = 1)
    {
        return Enumerable
            .Range(0, count)
            .Select(index => $"{index + 1}");
    }

    /// <summary>
    /// Creates a random long number an parses it to string.
    /// </summary>
    /// <returns>A random long number as string.</returns>
    public static string CreateRandomLongAsString()
    {
        return _faker.Random.Long().ToString(CultureInfo.InvariantCulture);
    }
}
