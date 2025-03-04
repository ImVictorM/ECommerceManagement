using System.Net.Http.Headers;

using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IntegrationTests.TestUtils.Extensions.Http;

/// <summary>
/// Extension methods for the HttpClient.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Adds an Authorization header containing the given token.
    /// </summary>
    /// <param name="httpClient">The current client.</param>
    /// <param name="token">The token to be set.</param>
    public static void SetJwtBearerAuthorizationHeader(
        this HttpClient httpClient,
        string token
    )
    {
        var authenticationValue = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            token
        );

        httpClient.DefaultRequestHeaders.Authorization = authenticationValue;
    }

    /// <summary>
    /// Adds the X-Idempotency-Key header to the current http client
    /// containing the specified idempotency key.
    /// </summary>
    /// <param name="httpClient">The current client.</param>
    /// <param name="idempotencyKey">
    /// The idempotency key to be added to the header.
    /// </param>
    public static void SetIdempotencyKeyHeader(
        this HttpClient httpClient,
        string idempotencyKey
    )
    {
        httpClient.DefaultRequestHeaders.Add(
            "X-Idempotency-Key",
            idempotencyKey
        );
    }

    /// <summary>
    /// Adds the X-Provider-Signature header to the current http client
    /// containing the specified provider signature.
    /// </summary>
    /// <param name="httpClient">The current client.</param>
    /// <param name="providerSignature">
    /// The provider signature to add to the header.
    /// </param>
    public static void SetProviderSignatureHeader(
        this HttpClient httpClient,
        string providerSignature
    )
    {
        httpClient.DefaultRequestHeaders.Add(
             "X-Provider-Signature",
             providerSignature
        );
    }
}
