using Domain.ProductReviewAggregate;
using Domain.UserAggregate.ValueObjects;

using Contracts.ProductReviews;

using WebApi.ProductReviews;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.ProductReviews;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.ProductReviews;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.ProductReviews;

/// <summary>
/// Integration tests for the get customer product reviews feature.
/// </summary>
public class GetCustomerProductReviewsTests : BaseIntegrationTest
{
    private readonly IUserSeed _seedUser;
    private readonly IProductReviewSeed _seedProductReview;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="GetCustomerProductReviewsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCustomerProductReviewsTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<IUserSeed>();
        _seedProductReview = SeedManager.GetSeed<IProductReviewSeed>();
    }

    /// <summary>
    /// Verifies it is not possible to retrieve reviews without
    /// authentication.
    /// </summary>
    [Fact]
    public async Task GetCustomerProductReviews_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existentUserId = _seedUser
            .GetEntityId(UserSeedType.CUSTOMER)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductReviewEndpoints.GetCustomerProductReviews),
            new
            {
                userId = existentUserId
            }
        );

        var result = await RequestService.CreateClient().GetAsync(endpoint);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to retrieve reviews when the
    /// authenticated user is not the reviews' owner or admin.
    /// </summary>
    [Fact]
    public async Task GetCustomerProductReviews_WithoutSelfOrAdminAuthentication_ReturnsForbidden()
    {
        var currentCustomerType = UserSeedType.CUSTOMER;
        var otherCustomerType = UserSeedType.CUSTOMER_WITH_ADDRESS;
        var otherCustomerId = _seedUser
            .GetEntityId(otherCustomerType)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductReviewEndpoints.GetCustomerProductReviews),
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
    /// Verifies it is possible to retrieve reviews when the authenticated user
    /// is the reviews' owner or admin.
    /// </summary>
    /// <param name="currentUserType">The current authenticate user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.ADMIN)]
    public async Task GetCustomerProductReviews_WithSelfOrAdminAuthentication_ReturnsOk(
        UserSeedType currentUserType
    )
    {
        var userToRetrieveReviewType = UserSeedType.CUSTOMER;
        var userToRetrieveReviewId = _seedUser
            .GetEntityId(userToRetrieveReviewType);

        var expectedReviews = GetActiveCustomerReviews(
            userToRetrieveReviewId
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductReviewEndpoints.GetCustomerProductReviews),
            new
            {
                userId = userToRetrieveReviewId.ToString()
            }
        );

        var client = await RequestService.LoginAsAsync(currentUserType);
        var response = await client.GetAsync(endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductReviewResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedReviews);
    }

    private IEnumerable<ProductReview> GetActiveCustomerReviews(
        UserId userId
    )
    {
        return _seedProductReview.ListAll(r => r.UserId == userId && r.IsActive);
    }
}
