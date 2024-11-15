using FluentAssertions;
using SharedKernel.Extensions;

namespace SharedKernel.UnitTests.Extensions;

/// <summary>
/// Tests for the <see cref="StringExtensions"/> extension methods.
/// </summary>
public class StringExtensionsTests
{
    /// <summary>
    /// Tests if the <see cref="StringExtensions.ToLowerSnakeCase(string)"/> extension method
    /// converts a string to snake and lower case correctly.
    /// </summary>
    /// <param name="input">The string to be converted.</param>
    /// <param name="expectedOutput">The expected string output.</param>
    [Theory]
    [InlineData("camelCase", "camel_case")]
    [InlineData("PascalCase", "pascal_case")]
    [InlineData("kebab-case", "kebab_case")]
    [InlineData("UPPER", "upper")]
    public void StringExtensions_WhenConvertingToLowerAndSnakeCase_ConvertsCorrectlyAndReturnsIt(string input, string expectedOutput)
    {
        input.ToLowerSnakeCase().Should().Be(expectedOutput);
    }
}
