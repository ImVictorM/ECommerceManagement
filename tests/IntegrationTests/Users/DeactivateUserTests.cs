using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Users;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the deactivate user feature.
/// </summary>
public class DeactivateUserTests : BaseIntegrationTest
{
    private readonly IUserSeed _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeactivateUserTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<IUserSeed>();
    }

    /// <summary>
    /// Verifies when a customer tries to deactivate another user, the response
    /// status is Forbidden. Ensures that customers cannot deactivate other users
    /// regardless of the target user type.
    /// </summary>
    /// <param name="otherUserType">
    /// The other user type the customer is trying to deactivate.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.ADMIN)]
    public async Task DeactivateUser_WhenCustomerTriesToDeactivateAnotherUser_ReturnsForbidden(
        UserSeedType otherUserType
    )
    {
        var idOtherUser = _seedUser.GetEntityId(otherUserType).ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = idOtherUser }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies that when an admin attempts to deactivate another admin,
    /// the response status is Forbidden. Ensures that admins do not have
    /// permission to deactivate other admins.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateAnotherAdmin_ReturnsForbidden()
    {
        var idOtherAdmin = _seedUser
            .GetEntityId(UserSeedType.OTHER_ADMIN)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = idOtherAdmin }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies when an admin attempts to deactivate themselves, the response
    /// status is Forbidden. Ensures that admins cannot perform self-deactivation.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateThemselves_ReturnsForbidden()
    {
        var adminType = UserSeedType.ADMIN;
        var adminId = _seedUser.GetEntityId(adminType).ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = adminId }
        );

        var client = await RequestService.LoginAsAsync(adminType);

        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies when an admin attempts to deactivate another customer, the
    /// operation succeeds and the customer is marked as inactive.
    /// Verifies that the deactivated user is included in the list of inactive
    /// users.
    /// </summary>
    /// <param name="seedCustomerType">
    /// The type of customer the admin is trying to deactivate.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateAnotherUser_ReturnsNoContent(
        UserSeedType seedCustomerType
    )
    {
        var idCustomerToDeactivate = _seedUser
            .GetEntityId(seedCustomerType)
            .ToString();

        var endpointDeactivate = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = idCustomerToDeactivate }
        );

        var endpointGetDeactivateUsers = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUsers),
            new { active = "false" }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseDeactivate = await client.DeleteAsync(
            endpointDeactivate
        );
        var responseGetInactiveUsers = await client.GetAsync(
            endpointGetDeactivateUsers
        );

        var responseGetInactiveUsersContent = await responseGetInactiveUsers.Content
            .ReadRequiredFromJsonAsync<IEnumerable<UserResponse>>();

        responseDeactivate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetInactiveUsers.StatusCode.Should().Be(HttpStatusCode.OK);
        responseGetInactiveUsersContent
            .Select(u => u.Id)
            .Should()
            .Contain(idCustomerToDeactivate);
    }

    /// <summary>
    /// Verifies when a customer deactivates themselves, the operation succeeds
    /// and the user is marked as inactive. Verifies that the self-deactivated
    /// user is included in the list of inactive users.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenCustomerTriesToDeactivateThemselves_ReturnsNoContent()
    {
        var customerType = UserSeedType.CUSTOMER;
        var customerId = _seedUser.GetEntityId(customerType).ToString();

        var endpointDeactivate = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = customerId }
        );

        var endpointGetDeactivateUsers = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUsers),
            new { active = "false" }
        );

        var clientCustomer = await RequestService.LoginAsAsync(customerType);
        var clientAdmin = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseDeactivate = await clientCustomer.DeleteAsync(
            endpointDeactivate
        );
        var responseGetInactiveUsers = await clientAdmin.GetAsync(
            endpointGetDeactivateUsers
        );

        var responseGetInactiveUsersContent = await responseGetInactiveUsers.Content
            .ReadRequiredFromJsonAsync<IEnumerable<UserResponse>>();

        responseDeactivate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetInactiveUsers.StatusCode.Should().Be(HttpStatusCode.OK);
        responseGetInactiveUsersContent
            .Select(u => u.Id)
            .Should()
            .Contain(customerId);
    }
}
