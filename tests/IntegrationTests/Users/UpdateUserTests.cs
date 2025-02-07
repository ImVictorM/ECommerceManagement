using Domain.UserAggregate;

using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Users.TestUtils;
using IntegrationTests.TestUtils.Constants;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for update user functionalities.
/// </summary>
public class UpdateUserTests : BaseIntegrationTest
{
    private readonly IDataSeed<UserSeedType, User> _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateUserTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateUserTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
    }

    /// <summary>
    /// Tests that a user with customer privileges is unable to update another user's information.
    /// </summary>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task UpdateUser_WhenCustomerTriesToUpdateSomeoneElse_ReturnsForbidden(UserSeedType otherUserType)
    {
        var otherUser = _seedUser.GetByType(otherUserType);
        var request = UpdateUserRequestUtils.CreateRequest(name: "a dumb name for another user");
        var endpoint = TestConstants.UserEndpoints.UpdateUser(otherUser.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.PutAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that a customer can update their own information.
    /// Verifies that the response status and checks that the user's details are updated correctly.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenCustomerTriesToUpdateThemselves_ReturnsNoContentAndUpdatesUser()
    {
        var request = UpdateUserRequestUtils.CreateRequest(name: "marcos rogério", phone: "19982748242", email: "marcao@email.com");

        var userToBeUpdated = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var endpoint = TestConstants.UserEndpoints.UpdateUser(userToBeUpdated.Id.ToString());
        var updateResponse = await RequestService.Client.PutAsJsonAsync(endpoint, request);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await RequestService.Client.GetAsync("/users/self");
        var getResponseContent = await getResponse.Content.ReadRequiredFromJsonAsync<UserResponse>();

        getResponseContent.Should().NotBeNull();
        getResponseContent.Name.Should().Be(request.Name);
        getResponseContent.Phone.Should().Be(request.Phone);
        getResponseContent.Email.ToString().Should().Be(request.Email);
    }

    /// <summary>
    /// Tests that an admin user cannot update the information of another admin user.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenAdminTriesToUpdateAnotherAdmin_ReturnsForbidden()
    {
        var otherAdmin = _seedUser.GetByType(UserSeedType.OTHER_ADMIN);
        var request = UpdateUserRequestUtils.CreateRequest(name: "a dumb admin");
        var endpoint = TestConstants.UserEndpoints.UpdateUser(otherAdmin.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.PutAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that a customer attempting to update their email to one that already exists in the system receives a conflict response.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenCustomerTriesToUpdateEmailWithExistingOne_ReturnsConflict()
    {
        var anotherUserEmail = _seedUser.GetByType(UserSeedType.CUSTOMER_WITH_ADDRESS).Email;
        var requestContainingAnotherUserEmail = UpdateUserRequestUtils.CreateRequest(email: anotherUserEmail.ToString());

        var userToBeUpdated = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var endpoint = TestConstants.UserEndpoints.UpdateUser(userToBeUpdated.Id.ToString());
        var updateResponse = await RequestService.Client.PutAsJsonAsync(endpoint, requestContainingAnotherUserEmail);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    /// <summary>
    /// Tests that an admin user can successfully update a customer's information.
    /// Verifies that the update succeeds and checks that the user's details are updated correctly.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenAdminTriesToUpdateCustomer_ReturnsNoContentAndUpdatesUser()
    {
        var customerToBeUpdated = _seedUser.GetByType(UserSeedType.CUSTOMER);
        var request = UpdateUserRequestUtils.CreateRequest(name: "User new name", phone: "19982748242", email: "newemail@email.com");
        var updateEndpoint = TestConstants.UserEndpoints.UpdateUser(customerToBeUpdated.Id.ToString());
        var getEndpoint = TestConstants.UserEndpoints.GetUserById(customerToBeUpdated.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var updateResponse = await RequestService.Client.PutAsJsonAsync(updateEndpoint, request);

        var getResponse = await RequestService.Client.GetAsync(getEndpoint);
        var getResponseContent = await getResponse.Content.ReadRequiredFromJsonAsync<UserResponse>();

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponseContent.Should().NotBeNull();
        getResponseContent.Name.Should().Be(request.Name);
        getResponseContent.Phone.Should().Be(request.Phone);
        getResponseContent.Email.ToString().Should().Be(request.Email);
    }
}
