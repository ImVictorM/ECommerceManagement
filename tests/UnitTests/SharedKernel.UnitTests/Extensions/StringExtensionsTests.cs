using SharedKernel.Extensions;

using FluentAssertions;

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

    /// <summary>
    /// Tests if the <see cref="StringExtensions.ToUpperSnakeCase(string)"/> extension method
    /// converts a string to snake and upper case correctly.
    /// </summary>
    /// <param name="input">The string to be converted.</param>
    /// <param name="expectedOutput">The expected string output.</param>
    [Theory]
    [InlineData("camelCase", "CAMEL_CASE")]
    [InlineData("PascalCase", "PASCAL_CASE")]
    [InlineData("kebab-case", "KEBAB_CASE")]
    [InlineData("lower", "LOWER")]
    [InlineData("UPPER", "UPPER")]
    [InlineData("UPPER-CASE", "UPPER_CASE")]
    public void StringExtensions_WhenConvertingToUpperAndSnakeCase_ConvertsCorrectlyAndReturnsIt(string input, string expectedOutput)
    {
        input.ToUpperSnakeCase().Should().Be(expectedOutput);
    }
}
