using Domain.UserAggregate;

using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;

using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for user deactivation scenarios, covering role-based access restrictions and expected
/// responses when attempting to deactivate users under various conditions.
/// </summary>
public class DeactivateUserTests : BaseIntegrationTest
{
    private readonly IDataSeed<UserSeedType, User> _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeactivateUserTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
    }

    /// <summary>
    /// Tests that when a customer tries to deactivate another user, the response status is Forbidden.
    /// Ensures that customers cannot deactivate other users regardless of the target user type.
    /// </summary>
    /// <param name="otherUserType">The other user type the customer is trying to deactivate.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.ADMIN)]
    public async Task DeactivateUser_WhenCustomerTriesToDeactivateAnotherUser_ReturnsForbidden(UserSeedType otherUserType)
    {
        var otherUser = _seedUser.GetByType(otherUserType);
        var endpoint = TestConstants.UserEndpoints.DeactivateUser(otherUser.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an admin attempts to deactivate another admin, the response status is Forbidden.
    /// Ensures that admins do not have permission to deactivate other admins.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateAnotherAdmin_ReturnsForbidden()
    {
        var otherAdmin = _seedUser.GetByType(UserSeedType.OTHER_ADMIN);
        var endpoint = TestConstants.UserEndpoints.DeactivateUser(otherAdmin.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an admin attempts to deactivate themselves, the response status is Forbidden.
    /// Ensures that admins cannot perform self-deactivation.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateThemselves_ReturnsForbidden()
    {
        var authenticatedAdmin = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var endpoint = TestConstants.UserEndpoints.DeactivateUser(authenticatedAdmin.Id.ToString());

        var response = await RequestService.Client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an admin attempts to deactivate another customer, the operation succeeds and the
    /// customer is marked as inactive. Verifies that the deactivated user is included in the list of inactive users.
    /// </summary>
    /// <param name="seedCustomerType">The type of customer the admin is trying to deactivate.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateAnotherUser_DeactivateTheUserAndReturnsNoContent(UserSeedType seedCustomerType)
    {
        var customerToDeactivate = _seedUser.GetByType(seedCustomerType);
        var deactivateEndpoint = TestConstants.UserEndpoints.DeactivateUser(customerToDeactivate.Id.ToString());
        var listDeactivateUsersEndpoint = $"{TestConstants.UserEndpoints.GetAllUsers}?active=false";

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseDeactivate = await RequestService.Client.DeleteAsync(deactivateEndpoint);
        var responseGetInactiveUsers = await RequestService.Client.GetAsync(listDeactivateUsersEndpoint);

        var responseGetInactiveUsersContent = await responseGetInactiveUsers.Content.ReadRequiredFromJsonAsync<IEnumerable<UserResponse>>();

        responseDeactivate.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        responseGetInactiveUsers.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseGetInactiveUsersContent.Select(u => u.Id).Should().Contain(customerToDeactivate.Id.ToString());
    }

    /// <summary>
    /// Tests that when a customer deactivates themselves, the operation succeeds and the user is marked as
    /// inactive. Verifies that the self-deactivated user is included in the list of inactive users.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenCustomerTriesToDeactivateThemselves_DeactivateThemAndReturnsNoContent()
    {
        var customer = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var deactivateEndpoint = TestConstants.UserEndpoints.DeactivateUser(customer.Id.ToString());
        var listDeactivateUsersEndpoint = $"{TestConstants.UserEndpoints.GetAllUsers}?active=false";

        var responseDeactivate = await RequestService.Client.DeleteAsync(deactivateEndpoint);

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseGetInactiveUsers = await RequestService.Client.GetAsync(listDeactivateUsersEndpoint);

        var responseGetInactiveUsersContent = await responseGetInactiveUsers.Content.ReadFromJsonAsync<IEnumerable<UserResponse>>();

        responseDeactivate.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        responseGetInactiveUsers.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseGetInactiveUsersContent!.Select(u => u.Id).Should().Contain(customer.Id.ToString());
    }
}
