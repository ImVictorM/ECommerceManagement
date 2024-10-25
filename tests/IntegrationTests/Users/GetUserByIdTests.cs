using System.Net;
using System.Net.Http.Json;
using Contracts.Users;
using Domain.UserAggregate;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Extensions.Users;
using IntegrationTests.TestUtils.Seeds;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the process of getting a user by identifier.
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
    /// A list of users contained in the test database.
    /// </summary>
    /// <returns>A list of seed users.</returns>
    public static IEnumerable<object[]> SeedUsers()
    {
        foreach (var user in UserSeed.ListUsers())
        {
            yield return new object[] { user };
        }
    }

    /// <summary>
    /// Tests if it is possible to get an user authenticated as administrator.
    /// </summary>
    /// <param name="user">The user to get.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(SeedUsers))]
    public async Task GetUserById_WhenRequesterIsAdmin_RetunsOkContainingTheUser(User user)
    {
        var (_, Token) = await LoginAs(SeedAvailableUsers.Admin);

        Client.SetJwtBearerAuthorizationHeader(Token);
 
        var response = await Client.GetAsync($"/users/{user.Id}");

        var responseContent = await response.Content.ReadFromJsonAsync<UserByIdResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent!.EnsureUserCorrespondsTo(user);
    }

    /// <summary>
    /// Tests if a customer cannot use this resource.
    /// </summary>
    /// <param name="user">The user to be queried.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(SeedUsers))]
    public async Task GetUserById_WhenRequesterIsNormalCustomer_ReturnForbidden(User user)
    {
        var (_, Token) = await LoginAs(SeedAvailableUsers.User1);

        Client.SetJwtBearerAuthorizationHeader(Token);

        var response = await Client.GetAsync($"/users/{user.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests if it is not possible to query for an user without authentication.
    /// </summary>
    /// <param name="user">The user to be queried.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(SeedUsers))]
    public async Task GetUserById_WhenAuthenticationTokenIsNotGiven_ReturnsUnauthorized(User user)
    {
        var response = await Client.GetAsync($"/users/{user.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
