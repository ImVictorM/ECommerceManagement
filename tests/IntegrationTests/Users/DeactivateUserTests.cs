using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;

using Contracts.Users;

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
    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeactivateUserTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests that when a customer tries to deactivate another user, the response status is Forbidden.
    /// Ensures that customers cannot deactivate other users regardless of the target user type.
    /// </summary>
    /// <param name="otherUserType">The other user type the customer is trying to deactivate.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    [InlineData(SeedAvailableUsers.Admin)]
    public async Task DeactivateUser_WhenCustomerTriesToDeactivateAnotherUser_ReturnsForbidden(SeedAvailableUsers otherUserType)
    {
        var otherUser = UserSeed.GetSeedUser(otherUserType);

        await Client.LoginAs(SeedAvailableUsers.Customer);
        var response = await Client.DeleteAsync($"/users/{otherUser.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an admin attempts to deactivate another admin, the response status is Forbidden.
    /// Ensures that admins do not have permission to deactivate other admins.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateAnotherAdmin_ReturnsForbidden()
    {
        var otherAdmin = UserSeed.GetSeedUser(SeedAvailableUsers.OtherAdmin);

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var response = await Client.DeleteAsync($"/users/{otherAdmin.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an admin attempts to deactivate themselves, the response status is Forbidden.
    /// Ensures that admins cannot perform self-deactivation.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateThemselves_ReturnsForbidden()
    {
        var authenticatedAdmin = await Client.LoginAs(SeedAvailableUsers.Admin);
        var response = await Client.DeleteAsync($"/users/{authenticatedAdmin.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an admin attempts to deactivate another customer, the operation succeeds and the
    /// customer is marked as inactive. Verifies that the deactivated user is included in the list of inactive users.
    /// </summary>
    /// <param name="seedCustomerType">The type of customer the admin is trying to deactivate.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.Customer)]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    public async Task DeactivateUser_WhenAdminTriesToDeactivateAnotherUser_DeactivateTheUserAndReturnsNoContent(SeedAvailableUsers seedCustomerType)
    {
        var customerToDeactivate = UserSeed.GetSeedUser(seedCustomerType);

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var responseDeactivate = await Client.DeleteAsync($"/users/{customerToDeactivate.Id}");
        var responseGetInactiveUsers = await Client.GetAsync($"/users?active=false");

        var responseGetInactiveUsersContent = await responseGetInactiveUsers.Content.ReadFromJsonAsync<IEnumerable<UserResponse>>();

        responseDeactivate.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        responseGetInactiveUsers.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseGetInactiveUsersContent!.Select(u => u.Id).Should().Contain(customerToDeactivate.Id.ToString());
    }

    /// <summary>
    /// Tests that when a customer deactivates themselves, the operation succeeds and the user is marked as
    /// inactive. Verifies that the self-deactivated user is included in the list of inactive users.
    /// </summary>
    [Fact]
    public async Task DeactivateUser_WhenCustomerTriesToDeactivateThemselves_DeactivateThemAndReturnsNoContent()
    {
        var customer = await Client.LoginAs(SeedAvailableUsers.Customer);
        var responseDeactivate = await Client.DeleteAsync($"/users/{customer.Id}");

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var responseGetInactiveUsers = await Client.GetAsync($"/users?active=false");

        var responseGetInactiveUsersContent = await responseGetInactiveUsers.Content.ReadFromJsonAsync<IEnumerable<UserResponse>>();

        responseDeactivate.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        responseGetInactiveUsers.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseGetInactiveUsersContent!.Select(u => u.Id).Should().Contain(customer.Id.ToString());
    }
}
