using System.Collections.ObjectModel;
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
/// Integration tests for the process of getting a list of users.
/// </summary>
public class GetAllUsersTests : BaseIntegrationTest
{
    private const string BaseRequestUri = "/users";

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllUsersTests"/> class.
    /// </summary>
    /// <param name="webAppFactory">The test server.</param>
    public GetAllUsersTests(IntegrationTestWebAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    /// <summary>
    /// A list of request URIs and the corresponding users it should return.
    /// </summary>
    /// <returns>A list of URIs and users.</returns>
    public static IEnumerable<object[]> RequestUriWithExpectedUsers()
    {
        yield return new object[] { "/users?active=true", UserSeed.ListUsers(u => u.IsActive).ToList().AsReadOnly() };
        yield return new object[] { "/users?active=false", UserSeed.ListUsers(u => !u.IsActive).ToList().AsReadOnly() };
        yield return new object[] { "/users", UserSeed.ListUsers().ToList().AsReadOnly() };
    }

    /// <summary>
    /// Tests if it return a success status code when the requester is admin.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task GetAllUsers_WhenRequesterIsAdmin_ReturnsSuccess()
    {
        var (_, AuthToken) = await LoginAs(SeedAvailableUsers.Admin);

        Client.SetJwtBearerAuthorizationHeader(AuthToken);

        var response = await Client.GetAsync(BaseRequestUri);

        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Tests if it returns a forbidden status code when the requester is not an admin.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task GetAllUsers_WhenRequesterIsNotAdmin_ReturnsForbidden()
    {
        var (_, AuthToken) = await LoginAs(SeedAvailableUsers.User1);

        Client.SetJwtBearerAuthorizationHeader(AuthToken);

        var response = await Client.GetAsync(BaseRequestUri);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests if it returns an unauthorized status code when the requester is not authenticated.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task GetAllUsers_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        var response = await Client.GetAsync(BaseRequestUri);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests if it returns a correct list of users when the requester is an authenticated administrator.
    /// </summary>
    /// <param name="endpoint">The uri to be called. May contain parameters.</param>
    /// <param name="expectedUsers">The expected user list.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(RequestUriWithExpectedUsers))]
    public async Task GetAllUsers_WhenAuthorizedAndFilteringTheUsers_ReturnsOkContainingUsersQueried(
        string endpoint,
        ReadOnlyCollection<User> expectedUsers
    )
    {
        var (_, AuthToken) = await LoginAs(SeedAvailableUsers.Admin);

        Client.SetJwtBearerAuthorizationHeader(AuthToken);

        var response = await Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadFromJsonAsync<UserListResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent!.Users.Count().Should().Be(expectedUsers.Count);
        responseContent.Users.Select(u => u.Id).Should().BeEquivalentTo(expectedUsers.Select(eu => eu.Id.ToString()));
        responseContent.Users.EnsureUsersCorrespondTo(expectedUsers);
    }
}
