using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Users.TestUtils;

using WebApi.Users;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Users;

/// <summary>
/// Integration tests for the update user feature.
/// </summary>
public class UpdateUserTests : BaseIntegrationTest
{
    private readonly IUserSeed _seedUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateUserTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateUserTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<IUserSeed>();
    }

    /// <summary>
    /// Verifies that a customer is unable to update another user's information.
    /// </summary>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task UpdateUser_WhenCustomerTriesToUpdateSomeoneElse_ReturnsForbidden(
        UserSeedType otherUserType
    )
    {
        var idOtherUser = _seedUser.GetEntityId(otherUserType).ToString();
        var request = UpdateUserRequestUtils.CreateRequest(
            name: "a dumb name for another user"
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = idOtherUser }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies an admin cannot update the information of another admin.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenAdminTriesToUpdateAnotherAdmin_ReturnsForbidden()
    {
        var idOtherAdmin = _seedUser
            .GetEntityId(UserSeedType.OTHER_ADMIN)
            .ToString();

        var request = UpdateUserRequestUtils.CreateRequest(name: "a dumb admin");

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = idOtherAdmin }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies a conflict response is returned when a customer tries to update
    /// their email with an already existing email.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenCustomerTriesToUpdateEmailWithExistingOne_ReturnsConflict()
    {
        var userToBeUpdatedType = UserSeedType.CUSTOMER;

        var idUserToBeUpdated = _seedUser
            .GetEntityId(userToBeUpdatedType)
            .ToString();

        var anotherUserEmail = _seedUser
            .GetEntity(UserSeedType.CUSTOMER_WITH_ADDRESS)
            .Email;

        var requestContainingAnotherUserEmail = UpdateUserRequestUtils.CreateRequest(
            email: anotherUserEmail.ToString()
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = idUserToBeUpdated }
        );

        var client = await RequestService.LoginAsAsync(userToBeUpdatedType);

        var response = await client.PutAsJsonAsync(
            endpoint,
            requestContainingAnotherUserEmail
        );

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    /// <summary>
    /// Verifies a customer can update their own information.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenCustomerTriesToUpdateThemselves_ReturnsNoContent()
    {
        var userToBeUpdatedType = UserSeedType.CUSTOMER;
        var userToBeUpdatedId = _seedUser
            .GetEntityId(userToBeUpdatedType)
            .ToString();

        var request = UpdateUserRequestUtils.CreateRequest(
            name: "marcos rogério",
            phone: "19982748242",
            email: "marcao@email.com"
        );

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = userToBeUpdatedId }
        );

        var endpointGetSelf = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserByAuthenticationToken)
        );

        var client = await RequestService.LoginAsAsync(userToBeUpdatedType);

        var responseUpdate = await client.PutAsJsonAsync(
            endpointUpdate,
            request
        );

        var responseGetUpdated = await client.GetAsync(
            endpointGetSelf
        );

        var responseGetUpdatedContent = await responseGetUpdated.Content
            .ReadRequiredFromJsonAsync<UserResponse>();

        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetUpdatedContent.Should().NotBeNull();
        responseGetUpdatedContent.Name.Should().Be(request.Name);
        responseGetUpdatedContent.Phone.Should().Be(request.Phone);
        responseGetUpdatedContent.Email.ToString().Should().Be(request.Email);
    }

    /// <summary>
    /// Verifies an admin can successfully update a customer's information.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenAdminTriesToUpdateCustomer_ReturnsNoContent()
    {
        var idCustomerToBeUpdated = _seedUser
            .GetEntityId(UserSeedType.CUSTOMER)
            .ToString();

        var request = UpdateUserRequestUtils.CreateRequest(
            name: "User new name",
            phone: "19982748242",
            email: "newemail@email.com"
        );

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = idCustomerToBeUpdated }
        );

        var endpointGetUserById = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserById),
            new { id = idCustomerToBeUpdated }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseUpdate = await client.PutAsJsonAsync(
            endpointUpdate,
            request
        );

        var responseGetUpdated = await client.GetAsync(endpointGetUserById);
        var responseGetUpdatedContent = await responseGetUpdated.Content
            .ReadRequiredFromJsonAsync<UserResponse>();

        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetUpdatedContent.Should().NotBeNull();
        responseGetUpdatedContent.Name.Should().Be(request.Name);
        responseGetUpdatedContent.Phone.Should().Be(request.Phone);
        responseGetUpdatedContent.Email.ToString().Should().Be(request.Email);
    }
}
