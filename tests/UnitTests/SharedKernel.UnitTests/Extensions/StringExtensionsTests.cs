using SharedKernel.Extensions;
using SharedKernel.Errors;

using FluentAssertions;

namespace SharedKernel.UnitTests.Extensions;

/// <summary>
/// Tests for the <see cref="StringExtensions"/> extension methods.
/// </summary>
public class StringExtensionsTests
{
    /// <summary>
    /// Verifies the <see cref="StringExtensions.ToLowerSnakeCase(string)"/>
    /// extension method converts a string to snake and lower case correctly.
    /// </summary>
    /// <param name="input">The string to be converted.</param>
    /// <param name="expectedOutput">The expected string output.</param>
    [Theory]
    [InlineData("camelCase", "camel_case")]
    [InlineData("PascalCase", "pascal_case")]
    [InlineData("kebab-case", "kebab_case")]
    [InlineData("UPPER", "upper")]
    public void ToLowerSnakeCase_WithDifferentTypeOfInputs_ConvertsToExpected(
        string input,
        string expectedOutput
    )
    {
        input.ToLowerSnakeCase().Should().Be(expectedOutput);
    }

    /// <summary>
    /// Verifies the <see cref="StringExtensions.ToUpperSnakeCase(string)"/>
    /// extension method converts a string to snake and upper case correctly.
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
    public void ToUpperSnakeCase_WithDifferentTypeOfInputs_ConvertsToExpected(
        string input,
        string expectedOutput
    )
    {
        input.ToUpperSnakeCase().Should().Be(expectedOutput);
    }

    /// <summary>
    /// Verifies the <see cref="StringExtensions.ToLongId(string)"/>
    /// extension method converts valid string representations of numbers to
    /// long correctly.
    /// </summary>
    /// <param name="input">The string to be converted.</param>
    /// <param name="expectedOutput">The expected long output.</param>
    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("-1", -1)]
    [InlineData("1234567890", 1234567890)]
    public void ToLongId_WithValidNumericStrings_ConvertsToExpected(
        string input,
        long expectedOutput
    )
    {
        input.ToLongId().Should().Be(expectedOutput);
    }

    /// <summary>
    /// Ensures that an <see cref="InvalidParseException"/> is thrown
    /// when the input string is not a valid long representation.
    /// </summary>
    /// <param name="input">The invalid string.</param>
    [Theory]
    [InlineData("abc")]
    [InlineData("12.34")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("9223372036854775808")] // Overflow case
    [InlineData("-9223372036854775809")] // Underflow case
    public void ToLongId_WithInvalidNumericStrings_ThrowsError(
        string input
    )
    {
        FluentActions
            .Invoking(input.ToLongId)
            .Should()
            .Throw<InvalidParseException>()
            .WithMessage("Error when converting a string to long");
    }
}
