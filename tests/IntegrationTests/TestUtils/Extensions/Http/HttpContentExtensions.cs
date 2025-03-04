using System.Net.Http.Json;

namespace IntegrationTests.TestUtils.Extensions.Http;

/// <summary>
/// Extensions for the <see cref="HttpContent"/> class.
/// </summary>
public static class HttpContentExtensions
{
    /// <summary>
    /// Reads a required data from json.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="content">The current content to read.</param>
    /// <returns>
    /// The deserialized content in the <typeparamref name="TResponse"/> format.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the deserialization fails.
    /// </exception>
    public static async Task<TResponse> ReadRequiredFromJsonAsync<TResponse>(
        this HttpContent content
    )
    {
        var parsedContent = await content.ReadFromJsonAsync<TResponse>();

        return parsedContent == null
            ? throw new InvalidOperationException(
                $"Failed to deserialize HTTP content to {typeof(TResponse).Name}." +
                $" The response body was empty or invalid."
            )
            : parsedContent;
    }
}
