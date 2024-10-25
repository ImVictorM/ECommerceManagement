

using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IntegrationTests.TestUtils.Extensions.HttpClient;

/// <summary>
/// Extension methods for the HttpClient.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Sets the client header to contain an authorization header with an JWT Bearer authentication token.
    /// </summary>
    /// <param name="httpClient">The current client.</param>
    /// <param name="token">The token to be set.</param>
    public static void SetJwtBearerAuthorizationHeader(this System.Net.Http.HttpClient httpClient, string token)
    {
        var authenticationValue = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

        httpClient.DefaultRequestHeaders.Authorization = authenticationValue;
    }
}
