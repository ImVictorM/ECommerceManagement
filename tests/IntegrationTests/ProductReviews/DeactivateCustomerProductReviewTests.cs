using Domain.UserAggregate.ValueObjects;
using Domain.ProductReviewAggregate;

using Contracts.ProductReviews;

using WebApi.ProductReviews;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.ProductReviews;
using IntegrationTests.TestUtils.Extensions.Http;

using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.ProductReviews;

/// <summary>
/// Integration tests for the deactivate customer product review feature.
/// </summary>
public class DeactivateCustomerProductReviewTests : BaseIntegrationTest
{
    private readonly IUserSeed _seedUser;
    private readonly IProductReviewSeed _seedProductReview;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="DeactivateCustomerProductReviewTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeactivateCustomerProductReviewTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<IUserSeed>();
        _seedProductReview = SeedManager.GetSeed<IProductReviewSeed>();
    }

    /// <summary>
    /// Verifies it is not possible to deactivate a review without
    /// authentication.
    /// </summary>
    [Fact]
    public async Task DeactivateCustomerProductReview_WithoutAuthentication_ReturnsUnauthorized()
    {
        var existentReview = _seedProductReview
            .GetEntity(ProductReviewSeedType.PENCIL_REVIEW_1);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductReviewEndpoints.DeactivateCustomerProductReview),
            new
            {
                userId = existentReview.UserId.ToString(),
                reviewId = existentReview.Id.ToString()
            }
        );

        var result = await RequestService.CreateClient().DeleteAsync(endpoint);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to deactivate a review when the
    /// authenticated user is not review owner or admin.
    /// </summary>
    [Fact]
    public async Task DeactivateCustomerProductReview_WithoutSelfOrAdminAuthentication_ReturnsForbidden()
    {
        var currentCustomerType = UserSeedType.CUSTOMER;
        var otherCustomerType = UserSeedType.CUSTOMER_WITH_ADDRESS;
        var otherCustomerId = _seedUser.GetEntityId(otherCustomerType);
        var otherCustomerProductReviewId = GetFirstProductReview(otherCustomerId).Id;

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductReviewEndpoints.DeactivateCustomerProductReview),
            new
            {
                userId = otherCustomerId.ToString(),
                reviewId = otherCustomerProductReviewId.ToString()
            }
        );

        var client = await RequestService.LoginAsAsync(currentCustomerType);
        var result = await client.DeleteAsync(endpoint);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies a not found response is returned when the review
    /// to be deactivated does not exist.
    /// </summary>
    /// <param name="currentUserType">The current authenticate user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.ADMIN)]
    public async Task DeactivateCustomerProductReview_WithNonExistentReview_ReturnsNotFound(
        UserSeedType currentUserType
    )
    {
        var userToRetrieveReviewType = UserSeedType.CUSTOMER;
        var userToRetrieveReviewId = _seedUser
            .GetEntityId(userToRetrieveReviewType);

        var nonExistentReviewId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerProductReviewEndpoints.DeactivateCustomerProductReview),
            new
            {
                userId = userToRetrieveReviewId.ToString(),
                reviewId = nonExistentReviewId
            }
        );

        var client = await RequestService.LoginAsAsync(currentUserType);
        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies it is possible to deactivate a review when the
    /// authenticated user is the review owner or admin.
    /// </summary>
    /// <param name="currentUserType">The current authenticate user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.ADMIN)]
    public async Task DeactivateCustomerProductReview_WithSelfOrAdminAuthentication_ReturnsNoContent(
        UserSeedType currentUserType
    )
    {
        var userReviewOwnerType = UserSeedType.CUSTOMER;
        var userReviewOwnerId = _seedUser.GetEntityId(userReviewOwnerType);
        var reviewToBeDeactivatedId = GetFirstProductReview(userReviewOwnerId).Id;

        var endpointDelete = LinkGenerator.GetPathByName(
            nameof(CustomerProductReviewEndpoints.DeactivateCustomerProductReview),
            new
            {
                userId = userReviewOwnerId.ToString(),
                reviewId = reviewToBeDeactivatedId.ToString()
            }
        );

        var endpointGetCustomerProductReviews = LinkGenerator.GetPathByName(
            nameof(CustomerProductReviewEndpoints.GetCustomerProductReviews),
            new
            {
                userId = userReviewOwnerId.ToString(),
            }
        );

        var client = await RequestService.LoginAsAsync(currentUserType);

        var responseDelete = await client.DeleteAsync(endpointDelete);

        var responseGetCustomerProductReviews = await client.GetAsync(
            endpointGetCustomerProductReviews
        );

        var responseGetCustomerProductReviewsContent = await
            responseGetCustomerProductReviews.Content
                .ReadRequiredFromJsonAsync<IEnumerable<ProductReviewResponse>>();

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetCustomerProductReviewsContent
            .Select(f => f.Id)
            .Should()
            .NotContain(reviewToBeDeactivatedId.ToString());
    }

    private ProductReview GetFirstProductReview(UserId userId)
    {
        return _seedProductReview.ListAll(r => r.UserId == userId)[0];
    }
}
