using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Users;

using WebApi.Users;

using Xunit.Abstractions;
using FluentAssertions;
using System.Net;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the get user by authentication token feature.
/// </summary>
public class GetUserByAuthenticationTokenTests : BaseIntegrationTest
{
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="GetUserByAuthenticationTokenTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetUserByAuthenticationTokenTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserByAuthenticationToken)
        );
    }

    /// <summary>
    /// Verifies the user data is returned when the token is valid.
    /// </summary>
    /// <param name="userType">
    /// The user type to be authenticated and queried.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task GetUserByAuthenticationToken_WithValidToken_ReturnsOk(
        UserSeedType userType
    )
    {
        var authenticatedUser = await RequestService.LoginAsAsync(userType);

        var response = await RequestService.Client.GetAsync(_endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<UserResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent.EnsureUserCorrespondsTo(authenticatedUser);
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when the user is not
    /// authenticated.
    /// </summary>
    [Fact]
    public async Task GetUserByAuthenticationToken_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await RequestService.Client.GetAsync(_endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when the token is not valid.
    /// </summary>
    [Fact]
    public async Task GetUserByAuthenticationToken_WhenTokenIsInvalid_ReturnsUnauthorized()
    {
        RequestService.Client.SetJwtBearerAuthorizationHeader("token");

        var response = await RequestService.Client.GetAsync(_endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
