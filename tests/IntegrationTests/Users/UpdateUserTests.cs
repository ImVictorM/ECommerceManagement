using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
using IntegrationTests.Users.TestUtils;

using Contracts.Users;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for update user functionalities.
/// </summary>
public class UpdateUserTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateUserTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateUserTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests that a user with customer privileges is unable to update another user's information.
    /// </summary>
    [Theory]
    [InlineData(SeedAvailableUsers.Admin)]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    public async Task UpdateUser_WhenCustomerTriesToUpdateSomeoneElse_ReturnsForbidden(SeedAvailableUsers otherUserType)
    {
        var otherUser = UserSeed.GetSeedUser(otherUserType);
        var request = UpdateUserRequestUtils.CreateRequest(name: "a dumb name for another user");

        await Client.LoginAs(SeedAvailableUsers.Customer);

        var response = await Client.PutAsJsonAsync($"users/{otherUser.Id}", request);

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

        var userToBeUpdated = await Client.LoginAs(SeedAvailableUsers.Customer);
        var updateResponse = await Client.PutAsJsonAsync($"users/{userToBeUpdated.Id}", request);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await Client.GetAsync("/users/self");
        var getResponseContent = await getResponse.Content.ReadFromJsonAsync<UserResponse>();

        getResponseContent.Should().NotBeNull();
        getResponseContent!.Name.Should().Be(request.Name);
        getResponseContent.Phone.Should().Be(request.Phone);
        getResponseContent.Email.ToString().Should().Be(request.Email);
    }

    /// <summary>
    /// Tests that an admin user cannot update the information of another admin user.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenAdminTriesToUpdateAnotherAdmin_ReturnsForbidden()
    {
        var otherAdmin = UserSeed.GetSeedUser(SeedAvailableUsers.OtherAdmin);
        var request = UpdateUserRequestUtils.CreateRequest(name: "a dumb admin");

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var response = await Client.PutAsJsonAsync($"users/{otherAdmin.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that a customer attempting to update their email to one that already exists in the system receives a conflict response.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenCustomerTriesToUpdateEmailWithExistingOne_ReturnsConflict()
    {
        var anotherUserEmail = UserSeed.GetSeedUser(SeedAvailableUsers.CustomerWithAddress).Email;
        var requestContainingAnotherUserEmail = UpdateUserRequestUtils.CreateRequest(email: anotherUserEmail.ToString());

        var userToBeUpdated = await Client.LoginAs(SeedAvailableUsers.Customer);
        var updateResponse = await Client.PutAsJsonAsync($"users/{userToBeUpdated.Id}", requestContainingAnotherUserEmail);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    /// <summary>
    /// Tests that an admin user can successfully update a customer's information.
    /// Verifies that the update succeeds and checks that the user's details are updated correctly.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenAdminTriesToUpdateCustomer_ReturnsNoContentAndUpdatesUser()
    {
        var customerToBeUpdated = UserSeed.GetSeedUser(SeedAvailableUsers.Customer);
        var request = UpdateUserRequestUtils.CreateRequest(name: "User new name", phone: "19982748242", email: "newemail@email.com");

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var updateResponse = await Client.PutAsJsonAsync($"users/{customerToBeUpdated.Id}", request);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await Client.GetAsync($"users/{customerToBeUpdated.Id}");
        var getResponseContent = await getResponse.Content.ReadFromJsonAsync<UserResponse>();

        getResponseContent.Should().NotBeNull();
        getResponseContent!.Name.Should().Be(request.Name);
        getResponseContent.Phone.Should().Be(request.Phone);
        getResponseContent.Email.ToString().Should().Be(request.Email);
    }
}
