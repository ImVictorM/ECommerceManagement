namespace SharedKernel.Extensions;

/// <summary>
/// Define shared string extensions.
/// </summary>
public static class StringExtensions
{
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
