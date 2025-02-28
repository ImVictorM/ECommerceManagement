using Domain.UserAggregate;

using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
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
    private readonly IDataSeed<UserSeedType, User> _seedUser;

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
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
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
        var otherUser = _seedUser.GetByType(otherUserType);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = otherUser.Id.ToString() }
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
        var otherAdmin = _seedUser.GetByType(UserSeedType.OTHER_ADMIN);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = otherAdmin.Id.ToString() }
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
        var admin = _seedUser.GetByType(adminType);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = admin.Id.ToString() }
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
        var customerToDeactivate = _seedUser.GetByType(seedCustomerType);

        var endpointDeactivate = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = customerToDeactivate.Id.ToString() }
        );

        var endpointGetDeactivateUsers = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetAllUsers),
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
            .Contain(customerToDeactivate.Id.ToString());
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
        var customer = _seedUser.GetByType(customerType);

        var endpointDeactivate = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.DeactivateUser),
            new { id = customer.Id.ToString() }
        );

        var endpointGetDeactivateUsers = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetAllUsers),
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
            .Contain(customer.Id.ToString());
    }
}
