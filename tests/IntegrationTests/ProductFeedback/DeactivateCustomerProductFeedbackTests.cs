using Domain.UserAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using Contracts.ProductFeedback;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.ProductFeedback;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.ProductFeedback;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.ProductFeedback;

/// <summary>
/// Integration tests for the deactivate customer product feedback feature.
/// </summary>
public class DeactivateCustomerProductFeedbackTests : BaseIntegrationTest
{
    private readonly IUserSeed _seedUser;
    private readonly IProductFeedbackSeed _seedProductFeedback;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="DeactivateCustomerProductFeedbackTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeactivateCustomerProductFeedbackTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<IUserSeed>();
        _seedProductFeedback = SeedManager.GetSeed<IProductFeedbackSeed>();
    }

    /// <summary>
    /// Verifies it is not possible to deactivate customer product feedback without
    /// authentication.
    /// </summary>
    [Fact]
    public async Task DeactivateCustomerProductFeedback_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingProductFeedback = _seedProductFeedback
            .GetEntity(ProductFeedbackSeedType.PENCIL_FEEDBACK_1);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductFeedbackEndpoints.DeactivateCustomerProductFeedback),
            new
            {
                userId = existingProductFeedback.UserId.ToString(),
                feedbackId = existingProductFeedback.Id.ToString()
            }
        );

        var result = await RequestService.CreateClient().DeleteAsync(endpoint);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to deactivate customer product feedback when the
    /// authenticated user is not feedback owner or admin.
    /// </summary>
    [Fact]
    public async Task DeactivateCustomerProductFeedback_WithoutSelfOrAdminAuthentication_ReturnsForbidden()
    {
        var currentCustomerType = UserSeedType.CUSTOMER;
        var otherCustomerType = UserSeedType.CUSTOMER_WITH_ADDRESS;
        var otherCustomerId = _seedUser
            .GetEntityId(otherCustomerType);
        var otherCustomerProductFeedbackId = GetFirstProductFeedback(otherCustomerId).Id;

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductFeedbackEndpoints.DeactivateCustomerProductFeedback),
            new
            {
                userId = otherCustomerId.ToString(),
                feedbackId = otherCustomerProductFeedbackId.ToString()
            }
        );

        var client = await RequestService.LoginAsAsync(currentCustomerType);
        var result = await client.DeleteAsync(endpoint);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies a not found response is returned when customer product feedback
    /// to be deactivated does not exist.
    /// </summary>
    /// <param name="currentUserType">The current authenticate user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.ADMIN)]
    public async Task DeactivateCustomerProductFeedback_WithNonexistingFeedback_ReturnsNotFound(
        UserSeedType currentUserType
    )
    {
        var userToRetrieveFeedbackType = UserSeedType.CUSTOMER;
        var userToRetrieveFeedbackId = _seedUser
            .GetEntityId(userToRetrieveFeedbackType);

        var nonExistingFeedbackId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductFeedbackEndpoints.DeactivateCustomerProductFeedback),
            new
            {
                userId = userToRetrieveFeedbackId.ToString(),
                feedbackId = nonExistingFeedbackId
            }
        );

        var client = await RequestService.LoginAsAsync(currentUserType);
        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies it is possible to deactivate customer product feedback when the
    /// authenticated user is feedback owner or admin.
    /// </summary>
    /// <param name="currentUserType">The current authenticate user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.ADMIN)]
    public async Task DeactivateCustomerProductFeedback_WithSelfOrAdminAuthentication_ReturnsNoContent(
        UserSeedType currentUserType
    )
    {
        var userFeedbackOwnerType = UserSeedType.CUSTOMER;
        var userFeedbackOwnerId = _seedUser.GetEntityId(userFeedbackOwnerType);
        var feedbackToBeDeactivatedId = GetFirstProductFeedback(userFeedbackOwnerId).Id;

        var endpointDelete = LinkGenerator.GetPathByName(
            nameof(CustomerProductFeedbackEndpoints.DeactivateCustomerProductFeedback),
            new
            {
                userId = userFeedbackOwnerId.ToString(),
                feedbackId = feedbackToBeDeactivatedId.ToString()
            }
        );

        var endpointGetCustomerProductFeedback = LinkGenerator.GetPathByName(
            nameof(CustomerProductFeedbackEndpoints.GetCustomerProductFeedback),
            new
            {
                userId = userFeedbackOwnerId.ToString(),
            }
        );

        var client = await RequestService.LoginAsAsync(currentUserType);

        var responseDelete = await client.DeleteAsync(endpointDelete);

        var responseGetCustomerProductFeedback = await client.GetAsync(
            endpointGetCustomerProductFeedback
        );

        var responseGetCustomerProductFeedbackContent = await
            responseGetCustomerProductFeedback.Content
                .ReadRequiredFromJsonAsync<IEnumerable<ProductFeedbackResponse>>();

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetCustomerProductFeedbackContent
            .Select(f => f.Id)
            .Should()
            .NotContain(feedbackToBeDeactivatedId.ToString());
    }

    private DomainProductFeedback GetFirstProductFeedback(UserId userId)
    {
        return _seedProductFeedback.ListAll(f => f.UserId == userId)[0];
    }
}
