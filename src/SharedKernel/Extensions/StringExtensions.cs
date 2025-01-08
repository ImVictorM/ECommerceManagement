using SharedKernel.Errors;

namespace SharedKernel.Extensions;

/// <summary>
/// Define shared string extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts an string identifier to a long representation.
    /// </summary>
    /// <param name="str">The current identifier.</param>
    /// <returns>A long representation of the current string identifier.</returns>
    public static long ToLongId(this string str)
    {
        if (long.TryParse(str, out var id))
        {
            return id;
        }

        throw new
            InvalidParseException(message: "Error when converting a string to long")
            .WithContext("IdValue", str);
    }

    /// <summary>
    /// Converts a string to snake_case and lowercase using invariant culture.
    /// </summary>
    /// <param name="str">The string to be converted.</param>
    /// <returns>The string converted to snake_case and lowercase.</returns>
    public static string ToLowerSnakeCase(this string str)
    {
        if (str.Equals(str.ToUpperInvariant(), StringComparison.Ordinal))
        {
            return str.ToLowerInvariant();
        }

        return string
            .Concat(str.Select(ConvertChar))
            .ToLowerInvariant();
    }

    /// <summary>
    /// Converts a string to SNAKE_CASE and uppercase using invariant culture.
    /// </summary>
    /// <param name="str">The string to be converted.</param>
    /// <returns>The string converted to SNAKE_CASE and uppercase.</returns>
    public static string ToUpperSnakeCase(this string str)
    {
        var containsHyphen = str.Contains('-', StringComparison.OrdinalIgnoreCase);
        var isUpperCase = str.Equals(str.ToUpperInvariant(), StringComparison.Ordinal);
        var isLowerCase = str.Equals(str.ToLowerInvariant(), StringComparison.Ordinal);

        if (!containsHyphen)
        {
            if (isUpperCase)
            {
                return str;
            }
            else if (isLowerCase)
            {
                return str.ToUpperInvariant();
            }
        }

        if (isUpperCase)
        {
            return str.Replace('-', '_');
        }

        return string.Concat(str.Select(ConvertChar)).ToUpperInvariant();
    }

    private static string ConvertChar(char character, int index)
    {
        if (index == 0)
        {
            return $"{character}";
        }

        if (character.Equals('-'))
        {
            return "_";
        }

        if (char.IsUpper(character))
        {
            return $"_{character}";
        }

        return $"{character}";
    }
}
