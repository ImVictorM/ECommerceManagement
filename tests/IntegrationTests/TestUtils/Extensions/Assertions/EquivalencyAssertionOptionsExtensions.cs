using FluentAssertions;
using FluentAssertions.Equivalency;

namespace IntegrationTests.TestUtils.Extensions.Assertions;

/// <summary>
/// Extension methods for the <see cref="EquivalencyAssertionOptions"/> class.
/// </summary>
public static class EquivalencyAssertionOptionsExtensions
{
    /// <summary>
    /// Define a way of comparing DateTimeOffset properties. 
    /// </summary>
    /// <typeparam name="T">
    /// The object that contains DateTimeOffset properties that are comparable.
    /// </typeparam>
    /// <param name="options">The current options.</param>
    /// <returns>The options.</returns>
    public static EquivalencyAssertionOptions<T> ComparingWithDateTimeOffset<T>(
        this EquivalencyAssertionOptions<T> options
    )
    {
        options
            .Using<DateTimeOffset>(ctx =>
                ctx.Subject
                    .Should()
                    .BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(1)
            ))
            .WhenTypeIs<DateTimeOffset>();

        return options;
    }
}
