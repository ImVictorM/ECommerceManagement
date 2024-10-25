using System.Net;
using System.Net.Http.Json;
using Contracts.Users;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Extensions.Users;
using IntegrationTests.TestUtils.Seeds;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the process of getting a user by authentication token.
/// </summary>
public class GetUserByAuthenticationTokenTests : BaseIntegrationTest
{
    private const string RequestUri = "/users/self";

    /// <summary>
    /// Initiates a new instance of the <see cref="GetUserByAuthenticationTokenTests"/> class.
    /// </summary>
    /// <param name="webAppFactory">The test server.</param>
    public GetUserByAuthenticationTokenTests(IntegrationTestWebAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    /// <summary>
    /// Tests if when calling the endpoint with a correct authentication token it returns the user data.
    /// </summary>
    /// <param name="userType">The user type to be authenticated and queried.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [InlineData(SeedAvailableUsers.Admin)]
    [InlineData(SeedAvailableUsers.User1)]
    [InlineData(SeedAvailableUsers.User2)]
    public async Task GetUserByAuthenticationToken_WhenUserIsAuthorizedByToken_ReturnsOk(SeedAvailableUsers userType)
    {
        var (AuthenticatedUser, AuthenticationToken) = await LoginAs(userType);

        Client.SetJwtBearerAuthorizationHeader(AuthenticationToken);

        var response = await Client.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadFromJsonAsync<UserByIdResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.EnsureUserCorrespondsTo(AuthenticatedUser);
    }

    /// <summary>
    /// Tests if when trying to get user data without a token it returns unauthorized.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task GetUserByAuthenticationToken_WhenAuthorizationIsNotGiven_ReturnsUnauthorized()
    {
        var response = await Client.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests if when token is invalid it returns unauthorized response.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task GetUserByAuthenticationToken_WhenTokenIsInvalid_ReturnsUnauthorized()
    {
        Client.SetJwtBearerAuthorizationHeader("token");

        var response = await Client.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
