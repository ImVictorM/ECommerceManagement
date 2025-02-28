using Domain.UserAggregate;

using Contracts.Users;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
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
    private readonly IDataSeed<UserSeedType, User> _seedUser;

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
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
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
        var otherUser = _seedUser.GetByType(otherUserType);
        var request = UpdateUserRequestUtils.CreateRequest(
            name: "a dumb name for another user"
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = otherUser.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies a customer can update their own information.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenCustomerTriesToUpdateThemselves_ReturnsNoContentAndUpdatesUser()
    {
        var request = UpdateUserRequestUtils.CreateRequest(
            name: "marcos rogério",
            phone: "19982748242",
            email: "marcao@email.com"
        );

        var userToBeUpdated = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = userToBeUpdated.Id.ToString() }
        );

        var endpointGetSelf = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserByAuthenticationToken)
        );

        var responseUpdate = await RequestService.Client.PutAsJsonAsync(
            endpointUpdate,
            request
        );

        var responseGetUpdated = await RequestService.Client.GetAsync(
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
    /// Verifies an admin cannot update the information of another admin.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenAdminTriesToUpdateAnotherAdmin_ReturnsForbidden()
    {
        var otherAdmin = _seedUser.GetByType(UserSeedType.OTHER_ADMIN);
        var request = UpdateUserRequestUtils.CreateRequest(name: "a dumb admin");

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = otherAdmin.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies when a customer tries to update their email to one that already
    /// exists a conflict response is returned.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenCustomerTriesToUpdateEmailWithExistingOne_ReturnsConflict()
    {
        var anotherUserEmail = _seedUser
            .GetByType(UserSeedType.CUSTOMER_WITH_ADDRESS)
            .Email;

        var requestContainingAnotherUserEmail = UpdateUserRequestUtils.CreateRequest(
            email: anotherUserEmail.ToString()
        );

        var userToBeUpdated = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = userToBeUpdated.Id.ToString() }
        );

        var updateResponse = await RequestService.Client.PutAsJsonAsync(
            endpoint,
            requestContainingAnotherUserEmail
        );

        updateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    /// <summary>
    /// Verifies an admin can successfully update a customer's information.
    /// </summary>
    [Fact]
    public async Task UpdateUser_WhenAdminTriesToUpdateCustomer_ReturnsNoContentAndUpdatesUser()
    {
        var customerToBeUpdated = _seedUser.GetByType(UserSeedType.CUSTOMER);

        var request = UpdateUserRequestUtils.CreateRequest(
            name: "User new name",
            phone: "19982748242",
            email: "newemail@email.com"
        );

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.UpdateUser),
            new { id = customerToBeUpdated.Id.ToString() }
        );

        var endpointGetUserById = LinkGenerator.GetPathByName(
            nameof(UserEndpoints.GetUserById),
            new { id = customerToBeUpdated.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseUpdate = await RequestService.Client.PutAsJsonAsync(
            endpointUpdate,
            request
        );

        var responseGetUpdated = await RequestService.Client.GetAsync(endpointGetUserById);
        var responseGetUpdatedContent = await responseGetUpdated.Content
            .ReadRequiredFromJsonAsync<UserResponse>();

        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetUpdatedContent.Should().NotBeNull();
        responseGetUpdatedContent.Name.Should().Be(request.Name);
        responseGetUpdatedContent.Phone.Should().Be(request.Phone);
        responseGetUpdatedContent.Email.ToString().Should().Be(request.Email);
    }
}
