using System.Net;
using System.Net.Http.Json;
using Contracts.Users;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Extensions.Users;
using IntegrationTests.TestUtils.Seeds;
using Xunit.Abstractions;

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
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetUserByAuthenticationTokenTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests if when calling the endpoint with a correct authentication token it returns the user data.
    /// </summary>
    /// <param name="userType">The user type to be authenticated and queried.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.ADMIN)]
    [InlineData(SeedAvailableUsers.CUSTOMER)]
    [InlineData(SeedAvailableUsers.CUSTOMER_WITH_ADDRESS)]
    public async Task GetUserByAuthenticationToken_WhenUserIsAuthorizedByToken_ReturnsOk(SeedAvailableUsers userType)
    {
        var authenticatedUser = await Client.LoginAs(userType);

        var response = await Client.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadFromJsonAsync<UserResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.EnsureUserCorrespondsTo(authenticatedUser);
    }

    /// <summary>
    /// Tests if when trying to get user data without a token it returns unauthorized.
    /// </summary>
    [Fact]
    public async Task GetUserByAuthenticationToken_WhenAuthorizationIsNotGiven_ReturnsUnauthorized()
    {
        var response = await Client.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests if when token is invalid it returns unauthorized response.
    /// </summary>
    [Fact]
    public async Task GetUserByAuthenticationToken_WhenTokenIsInvalid_ReturnsUnauthorized()
    {
        Client.SetJwtBearerAuthorizationHeader("token");

        var response = await Client.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
