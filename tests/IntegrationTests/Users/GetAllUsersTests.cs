using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Extensions.Users;
using IntegrationTests.TestUtils.Seeds;

using Contracts.Users;

using Domain.UserAggregate;

using System.Collections.ObjectModel;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

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
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetAllUsersTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
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
    [Fact]
    public async Task GetAllUsers_WhenRequesterIsAdmin_ReturnsSuccess()
    {
        await Client.LoginAs(SeedAvailableUsers.Admin);

        var response = await Client.GetAsync(BaseRequestUri);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    /// <summary>
    /// Tests if it returns a forbidden status code when the requester is not an admin.
    /// </summary>
    [Fact]
    public async Task GetAllUsers_WhenRequesterIsNotAdmin_ReturnsForbidden()
    {
        await Client.LoginAs(SeedAvailableUsers.Customer);

        var response = await Client.GetAsync(BaseRequestUri);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests if it returns an unauthorized status code when the requester is not authenticated.
    /// </summary>
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
    [Theory]
    [MemberData(nameof(RequestUriWithExpectedUsers))]
    public async Task GetAllUsers_WhenAuthorizedAndFilteringTheUsers_ReturnsOkContainingUsersQueried(
        string endpoint,
        ReadOnlyCollection<User> expectedUsers
    )
    {
        await Client.LoginAs(SeedAvailableUsers.Admin);

        var response = await Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<UserResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent!.Count().Should().Be(expectedUsers.Count);
        responseContent!.Select(u => u.Id).Should().BeEquivalentTo(expectedUsers.Select(eu => eu.Id.ToString()));
        responseContent!.EnsureUsersCorrespondTo(expectedUsers);
    }
}
