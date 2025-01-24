using SharedKernel.ValueObjects;

using Bogus;

namespace SharedKernel.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Percentage"/> class.
/// </summary>
public static class PercentageUtils
{
    private static readonly Faker _faker = new();
    /// <summary>
    /// Creates a new instance of the <see cref="Percentage"/> class.
    /// </summary>
    /// <param name="value">The percentage value.</param>
    /// <returns>A new instance of the <see cref="Percentage"/> class.</returns>
    public static Percentage Create(int? value = null)
    {
        return Percentage.Create(
            value ?? _faker.Random.Int(1, 50)
        );
    }
}
