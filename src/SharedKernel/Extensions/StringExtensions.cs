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
        return string
            .Concat(str.Select((character, index) => index > 0 && char.IsUpper(character) ? $"_{character}" : $"{character}"))
            .ToLowerInvariant();
    }
}
