using Domain.UserAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Users;

using Contracts.Users;

using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the process of getting a user by identifier.
/// </summary>
public class GetUserByIdTests : BaseIntegrationTest
{
    private readonly IDataSeed<UserSeedType, User> _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetUserByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetUserByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
    }

    /// <summary>
    /// Tests if it is possible to get an user authenticated as administrator.
    /// </summary>
    /// <param name="userType">The user type to get.</param>
    [Theory]
    [InlineData(UserSeedType.OTHER_ADMIN)]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task GetUserById_WhenRequesterIsAdmin_ReturnsOkWithUser(UserSeedType userType)
    {
        var userToGet = _seedUser.GetByType(userType);

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.GetAsync($"/users/{userToGet.Id}");

        var responseContent = await response.Content.ReadRequiredFromJsonAsync<UserResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureUserCorrespondsTo(userToGet);
    }

    /// <summary>
    /// Tests if a customer cannot use this resource.
    /// </summary>
    /// <param name="userType">The user type to be queried.</param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task GetUserById_WhenRequesterIsNormalCustomer_ReturnForbidden(UserSeedType userType)
    {
        var userToGet = _seedUser.GetByType(userType);

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.GetAsync($"/users/{userToGet.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests if it is not possible to query for an user without authentication.
    /// </summary>
    [Fact]
    public async Task GetUserById_WhenAuthenticationTokenIsNotGiven_ReturnsUnauthorized()
    {
        var response = await RequestService.Client.GetAsync("/users/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests when the user being queried does not exist the response is NOT_FOUND and the error content is correct.
    /// </summary>
    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ReturnsNotFound()
    {
        var userNotFoundId = "5000";

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.GetAsync($"/users/{userNotFoundId}");

        var responseContent = await response.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("User Not Found");
        responseContent.Detail.Should().Be($"User with id {userNotFoundId} was not found");
    }
}
