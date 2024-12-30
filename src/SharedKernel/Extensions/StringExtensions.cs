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
            BaseException(
            message: "There was an error when converting the identifier",
            errorCode: ErrorCode.InvalidOperation,
            title: "Domain Error - Invalid Operation"
            ).WithContext("IdValue", str);
    }

    /// <summary>
    /// Converts a string to snake_case and lowercase using invariant culture
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

    private static string ConvertChar(char character, int index)
    {
        if (index == 0)
        {
            return $"{character}";
        }

        if (char.IsUpper(character))
        {
            return $"_{character}";
        }

        if (character.Equals('-'))
        {
            return "_";
        }

        return $"{character}";
    }
}
