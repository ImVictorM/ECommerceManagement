using System.Net.Http.Json;
using Contracts.Authentication;
using Domain.UserAggregate;
using FluentAssertions;
using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.TestUtils.Seeds;

namespace IntegrationTests.Common;

/// <summary>
/// Base component of integrations tests.
/// </summary>
public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    /// <summary>
    /// The client used to make requests.
    /// </summary>
    protected HttpClient HttpClient { get; init; }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseIntegrationTest"/> class.
    /// </summary>
    /// <param name="webAppFactory">The test server factory.</param>
    public BaseIntegrationTest(IntegrationTestWebAppFactory webAppFactory)
    {
        HttpClient = webAppFactory.CreateClient();
    }

    /// <summary>
    /// Login as a seed user and returns it containing a JWT token.
    /// </summary>
    /// <returns>the user authenticated and a JWT token.</returns>
    public async Task<(User User, string Token)> LoginAs(SeedAvailableUsers userType)
    {
        var (Email, Password) = UserSeed.GetUserAuthenticationCredentials(userType);

        var request = LoginRequestUtils.CreateRequest(Email, Password);

        var response = await HttpClient.PostAsJsonAsync("/auth/login", request);

        var responseContent = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

        responseContent.Should().NotBeNull();

        return (UserSeed.GetSeedUser(userType), responseContent!.Token);
    }
}
