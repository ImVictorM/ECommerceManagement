using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Extensions.Users;
using IntegrationTests.TestUtils.Seeds;

using Contracts.Users;

using Domain.UserAggregate;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the process of getting a user by identifier.
/// </summary>
public class GetUserByIdTests : BaseIntegrationTest
{
    /// <summary>
    /// A list of users contained in the test database.
    /// </summary>
    public static IEnumerable<object[]> AvailableUsers()
    {
        foreach (var user in UserSeed.ListUsers())
        {
            yield return new object[] { user };
        }
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="GetUserByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetUserByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests if it is possible to get an user authenticated as administrator.
    /// </summary>
    /// <param name="user">The user to get.</param>
    [Theory]
    [MemberData(nameof(AvailableUsers))]
    public async Task GetUserById_WhenRequesterIsAdmin_ReturnsOkWithUser(User user)
    {
        await Client.LoginAs(SeedAvailableUsers.ADMIN);
        var response = await Client.GetAsync($"/users/{user.Id}");

        var responseContent = await response.Content.ReadFromJsonAsync<UserResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent!.EnsureUserCorrespondsTo(user);
    }

    /// <summary>
    /// Tests if a customer cannot use this resource.
    /// </summary>
    /// <param name="user">The user to be queried.</param>
    [Theory]
    [MemberData(nameof(AvailableUsers))]
    public async Task GetUserById_WhenRequesterIsNormalCustomer_ReturnForbidden(User user)
    {
        await Client.LoginAs(SeedAvailableUsers.CUSTOMER);

        var response = await Client.GetAsync($"/users/{user.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests if it is not possible to query for an user without authentication.
    /// </summary>
    /// <param name="user">The user to be queried.</param>
    [Theory]
    [MemberData(nameof(AvailableUsers))]
    public async Task GetUserById_WhenAuthenticationTokenIsNotGiven_ReturnsUnauthorized(User user)
    {
        var response = await Client.GetAsync($"/users/{user.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests when the user being queried does not exist the response is NOT_FOUND and the error content is correct.
    /// </summary>
    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ReturnsBadRequest()
    {
        var userNotFoundId = "5000";

        await Client.LoginAs(SeedAvailableUsers.ADMIN);
        var response = await Client.GetAsync($"/users/{userNotFoundId}");

        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("User Not Found");
        responseContent.Detail.Should().Be($"User with id {userNotFoundId} was not found");
    }
}
