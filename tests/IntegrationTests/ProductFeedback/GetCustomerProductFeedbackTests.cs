using Domain.UserAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using Contracts.ProductFeedback;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.ProductFeedback;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.ProductFeedback;

using WebApi.ProductFeedback;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.ProductFeedback;

/// <summary>
/// Integration tests for the get customer product feedback feature.
/// </summary>
public class GetCustomerProductFeedbackTests : BaseIntegrationTest
{
    private readonly IUserSeed _seedUser;
    private readonly IProductFeedbackSeed _seedProductFeedback;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerProductFeedbackTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCustomerProductFeedbackTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<IUserSeed>();
        _seedProductFeedback = SeedManager.GetSeed<IProductFeedbackSeed>();
    }

    /// <summary>
    /// Verifies it is not possible to retrieve customer product feedback without
    /// authentication.
    /// </summary>
    [Fact]
    public async Task GetCustomerProductFeedback_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existingUserId = _seedUser
            .GetEntityId(UserSeedType.CUSTOMER)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductFeedbackEndpoints.GetCustomerProductFeedback),
            new
            {
                userId = existingUserId
            }
        );

        var result = await RequestService.CreateClient().GetAsync(endpoint);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to retrieve customer product feedback when the
    /// authenticated user is not feedback owner or admin.
    /// </summary>
    [Fact]
    public async Task GetCustomerProductFeedback_WithoutSelfOrAdminAuthentication_ReturnsForbidden()
    {
        var currentCustomerType = UserSeedType.CUSTOMER;
        var otherCustomerType = UserSeedType.CUSTOMER_WITH_ADDRESS;
        var otherCustomerId = _seedUser
            .GetEntityId(otherCustomerType)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductFeedbackEndpoints.GetCustomerProductFeedback),
            new
            {
                userId = otherCustomerId
            }
        );

        var client = await RequestService.LoginAsAsync(currentCustomerType);
        var result = await client.GetAsync(endpoint);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it is possible to retrieve customer product feedback when the
    /// authenticated user is feedback owner or admin.
    /// </summary>
    /// <param name="currentUserType">The current authenticate user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.ADMIN)]
    public async Task GetCustomerProductFeedback_WithSelfOrAdminAuthentication_ReturnsOk(
        UserSeedType currentUserType
    )
    {
        var userToRetrieveFeedbackType = UserSeedType.CUSTOMER;
        var userToRetrieveFeedbackId = _seedUser
            .GetEntityId(userToRetrieveFeedbackType);

        var expectedProductFeedbackResponse = GetUserProductFeedbackActiveItems(
            userToRetrieveFeedbackId
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductFeedbackEndpoints.GetCustomerProductFeedback),
            new
            {
                userId = userToRetrieveFeedbackId.ToString()
            }
        );

        var client = await RequestService.LoginAsAsync(currentUserType);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductFeedbackResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedProductFeedbackResponse);
    }

    private IEnumerable<DomainProductFeedback> GetUserProductFeedbackActiveItems(
        UserId userId
    )
    {
        return _seedProductFeedback.ListAll(f => f.UserId == userId && f.IsActive);
    }
}
