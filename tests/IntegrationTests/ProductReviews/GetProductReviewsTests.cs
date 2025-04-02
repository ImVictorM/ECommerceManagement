using Domain.ProductReviewAggregate;
using Domain.ProductAggregate.ValueObjects;

using Contracts.ProductReviews;

using WebApi.ProductReviews;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
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
/// Integration tests for the get product reviews feature.
/// </summary>
public class GetProductReviewsTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;
    private readonly IProductReviewSeed _seedProductReview;
    private readonly IUserSeed _seedUser;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductReviewsTests"/>
    /// class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetProductReviewsTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
        _seedProductReview = SeedManager.GetSeed<IProductReviewSeed>();
        _seedUser = SeedManager.GetSeed<IUserSeed>();

        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies it is possible to retrieve reviews for a given product.
    /// </summary>
    /// <param name="productSeedType">The product type.</param>
    [Theory]
    [InlineData(ProductSeedType.CHAIN_BRACELET)]
    [InlineData(ProductSeedType.PENCIL)]
    [InlineData(ProductSeedType.COMPUTER_ON_SALE)]
    [InlineData(ProductSeedType.TSHIRT)]
    public async Task GetProductReviews_WithDifferentProducts_ReturnsOk(
        ProductSeedType productSeedType
    )
    {
        var productId = _seedProduct.GetEntityId(productSeedType);

        var expectedProductReviews = GetActiveProductReviews(productId);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductReviewEndpoints.GetProductReviews),
            new
            {
                productId = productId.ToString(),
            }
        );

        var response = await _client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductReviewDetailedResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedProductReviews, _seedUser);
    }

    private IReadOnlyList<ProductReview> GetActiveProductReviews(
        ProductId productId
    )
    {
        return _seedProductReview
            .ListAll(r => r.ProductId == productId && r.IsActive);
    }
}
