using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contracts.Users;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the process of getting a user by id.
/// </summary>
public class GetUserByIdTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetUserByIdTests"/> class.
    /// </summary>
    /// <param name="webAppFactory">The test server.</param>
    public GetUserByIdTests(IntegrationTestWebAppFactory webAppFactory) : base(webAppFactory)
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
    public async Task GetUserById_WhenUserIsAuthorizedByToken_ReturnsOk(SeedAvailableUsers userType)
    {
        var (AuthenticatedUser, AuthenticationToken) = await LoginAs(userType);

        var authHeader = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, AuthenticationToken);

        HttpClient.DefaultRequestHeaders.Authorization = authHeader;

        var response = await HttpClient.GetAsync("/users/self");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadFromJsonAsync<UserByIdResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Email.Should().Be(AuthenticatedUser.Email.ToString());
        responseContent.Name.Should().Be(AuthenticatedUser.Name);
        responseContent.Roles.Count().Should().Be(AuthenticatedUser.UserRoles.Count);
        responseContent.Roles.Should().BeEquivalentTo(AuthenticatedUser.GetRoleNames());
        responseContent.Phone.Should().BeEquivalentTo(AuthenticatedUser.Phone);
        responseContent.Id.Should().Be(AuthenticatedUser.Id.ToString());
        responseContent.Addresses.Count().Should().Be(AuthenticatedUser.UserAddresses.Count);
    }

    /// <summary>
    /// Tests if when trying to get user data without a token it returns unauthorized.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task GetUserById_WhenAuthorizationIsNotGiven_ReturnsUnauthorized()
    {
        var response = await HttpClient.GetAsync("/users/self");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests if when token is invalid it returns unauthorized response.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task GetUserById_WhenTokenIsInvalid_ReturnsUnauthorized()
    {
        var authHeader = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, "token");

        HttpClient.DefaultRequestHeaders.Authorization = authHeader;

        var response = await HttpClient.GetAsync("/users/self");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
