using Domain.UserAggregate;

using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Users;

using WebApi.Users;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the get all users feature.
/// </summary>
public class GetAllUsersTests : BaseIntegrationTest
{
    private readonly IDataSeed<UserSeedType, User> _seedUser;
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllUsersTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetAllUsersTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
        _endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetAllUsers)
        );
    }

    /// <summary>
    /// Verifies an OK response is returned when the authenticated user has
    /// the admin role.
    /// </summary>
    [Fact]
    public async Task GetAllUsers_WithAdminRole_ReturnsOk()
    {
        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(_endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// Verifies an forbidden response is returned when the authenticated user
    /// does not have the admin role.
    /// </summary>
    [Fact]
    public async Task GetAllUsers_WithoutAdminRole_ReturnsForbidden()
    {
        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.GetAsync(_endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when the user is
    /// not authenticated.
    /// </summary>
    [Fact]
    public async Task GetAllUsers_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await RequestService.CreateClient().GetAsync(_endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies an OK response is returned containing a filtered subset of users
    /// when the authenticated user is an admin.
    /// </summary>
    /// <param name="activeFilter">The active filter.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public async Task GetAllUsers_WithAuthorizationAndActiveFilter_ReturnsOk(
        bool? activeFilter
    )
    {
        var endpoint = _endpoint;

        var expectedUsers = GetUsersFilteredByActive(activeFilter);

        if (activeFilter != null)
        {
            endpoint += $"?active={activeFilter}";
        }

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<UserResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
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
