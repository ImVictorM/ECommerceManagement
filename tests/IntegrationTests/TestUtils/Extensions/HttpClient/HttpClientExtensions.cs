

using System.Net.Http.Headers;
using Contracts.Authentication;
using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.UserAggregate;
using System.Net.Http.Json;
using System.Security.Authentication;

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

    /// <summary>
    /// Login as a seed user and returns it.
    /// </summary>
    /// <returns>The user authenticated.</returns>
    public static async Task<User> LoginAs(this System.Net.Http.HttpClient httpClient, SeedAvailableUsers userType)
    {
        var (Email, Password) = UserSeed.GetUserAuthenticationCredentials(userType);

        var request = LoginRequestUtils.CreateRequest(Email, Password);

        var response = await httpClient.PostAsJsonAsync("/auth/login", request);

        var responseContent =
            await response.Content.ReadFromJsonAsync<AuthenticationResponse>() ??
            throw new AuthenticationException($"Failed to login as {nameof(userType)}");

        httpClient.SetJwtBearerAuthorizationHeader(responseContent!.Token);

        return UserSeed.GetSeedUser(userType);
    }
}
