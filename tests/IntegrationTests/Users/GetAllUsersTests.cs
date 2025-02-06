using Domain.UserAggregate;

using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Users;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the process of getting a list of users.
/// </summary>
public class GetAllUsersTests : BaseIntegrationTest
{
    private readonly IDataSeed<UserSeedType, User> _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllUsersTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetAllUsersTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
    }

    /// <summary>
    /// Tests if it return a success status code when the requester is admin.
    /// </summary>
    [Fact]
    public async Task GetAllUsers_WhenRequesterIsAdmin_ReturnsSuccess()
    {
        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.GetAsync("/users");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    /// <summary>
    /// Tests if it returns a forbidden status code when the requester is not an admin.
    /// </summary>
    [Fact]
    public async Task GetAllUsers_WhenRequesterIsNotAdmin_ReturnsForbidden()
    {
        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.GetAsync("/users");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests if it returns an unauthorized status code when the requester is not authenticated.
    /// </summary>
    [Fact]
    public async Task GetAllUsers_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        var response = await RequestService.Client.GetAsync("/users");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies a correct list of users is returned when the requester is an authenticated administrator.
    /// </summary>
    /// <param name="activeFilter">The active filter.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public async Task GetAllUsers_WhenAuthorizedAndFilteringTheUsers_ReturnsOkContainingUsersQueried(
        bool? activeFilter
    )
    {
        var endpoint = "/users";
        var expectedUsers = GetUsersFilteredByActive(activeFilter);

        if (activeFilter != null)
        {
            endpoint += $"?active={activeFilter}";
        }

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<UserResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent.Count().Should().Be(expectedUsers.Count);
        responseContent.Select(u => u.Id).Should().BeEquivalentTo(expectedUsers.Select(eu => eu.Id.ToString()));
        responseContent.EnsureUsersCorrespondTo(expectedUsers);
    }

    private IReadOnlyList<User> GetUsersFilteredByActive(bool? activeFilter)
    {
        if (activeFilter == null)
        {
            return _seedUser.ListAll();
        }

        return _seedUser.ListAll(u => u.IsActive == activeFilter);
    }
}
