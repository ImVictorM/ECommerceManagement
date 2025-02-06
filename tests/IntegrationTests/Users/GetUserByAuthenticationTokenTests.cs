using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Users;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

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
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task GetUserByAuthenticationToken_WhenUserIsAuthorizedByToken_ReturnsOk(UserSeedType userType)
    {
        var authenticatedUser = await RequestService.LoginAsAsync(userType);

        var response = await RequestService.Client.GetAsync(RequestUri);
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<UserResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent.EnsureUserCorrespondsTo(authenticatedUser);
    }

    /// <summary>
    /// Tests if when trying to get user data without a token it returns unauthorized.
    /// </summary>
    [Fact]
    public async Task GetUserByAuthenticationToken_WhenAuthorizationIsNotGiven_ReturnsUnauthorized()
    {
        var response = await RequestService.Client.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests if when token is invalid it returns unauthorized response.
    /// </summary>
    [Fact]
    public async Task GetUserByAuthenticationToken_WhenTokenIsInvalid_ReturnsUnauthorized()
    {
        RequestService.Client.SetJwtBearerAuthorizationHeader("token");
        var response = await RequestService.Client.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
