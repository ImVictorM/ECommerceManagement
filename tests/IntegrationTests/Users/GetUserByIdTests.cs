using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Users;

using Contracts.Users;

using WebApi.Users;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the get user by id feature.
/// </summary>
public class GetUserByIdTests : BaseIntegrationTest
{
    private readonly IUserSeed _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetUserByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetUserByIdTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<IUserSeed>();
    }

    /// <summary>
    /// Verifies an ok response is returned contained the user data when
    /// authenticated as an administrator.
    /// </summary>
    /// <param name="userType">The user type to get.</param>
    [Theory]
    [InlineData(UserSeedType.OTHER_ADMIN)]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task GetUserById_WithAdminAuthentication_ReturnsOk(
        UserSeedType userType
    )
    {
        var userToRetrieve = _seedUser.GetEntity(userType);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserById),
            new { id = userToRetrieve.Id.ToString() }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<UserResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureUserCorrespondsTo(userToRetrieve);
    }

    /// <summary>
    /// Verifies a forbidden response is returned when a customer tries to
    /// use this feature.
    /// </summary>
    /// <param name="userToRetrieveType">
    /// The user to be retrieved type.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task GetUserById_WithoutAdminRole_ReturnForbidden(
        UserSeedType userToRetrieveType
    )
    {
        var idUserToRetrieve = _seedUser
            .GetEntityId(userToRetrieveType)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserById),
            new { id = idUserToRetrieve }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when the user is not
    /// authenticated.
    /// </summary>
    [Fact]
    public async Task GetUserById_WithoutAuthentication_ReturnsUnauthorized()
    {
        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserById),
            new { id = "1" }
        );

        var response = await RequestService
            .CreateClient()
            .GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies a not found response is returned when the user being queried
    /// does not exist.
    /// </summary>
    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ReturnsNotFound()
    {
        var userNotFoundId = "5000";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserById),
            new { id = userNotFoundId }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
