using Domain.ProductAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using Contracts.ProductFeedback;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
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
/// Integration tests for the get product feedback feature.
/// </summary>
public class GetProductFeedbackTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;
    private readonly IProductFeedbackSeed _seedProductFeedback;
    private readonly IUserSeed _seedUser;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="LeaveProductFeedbackTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetProductFeedbackTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
        _seedProductFeedback = SeedManager.GetSeed<IProductFeedbackSeed>();
        _seedUser = SeedManager.GetSeed<IUserSeed>();

        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies it is possible to retrieve feedback for a given product.
    /// </summary>
    /// <param name="productSeedType">The product type.</param>
    [Theory]
    [InlineData(ProductSeedType.CHAIN_BRACELET)]
    [InlineData(ProductSeedType.PENCIL)]
    [InlineData(ProductSeedType.COMPUTER_ON_SALE)]
    [InlineData(ProductSeedType.TSHIRT)]
    public async Task GetProductFeedback_WithDifferentProducts_ReturnsOk(
        ProductSeedType productSeedType
    )
    {
        var productId = _seedProduct.GetEntityId(productSeedType);

        var expectedProductFeedback = GetProductFeedbackItems(productId);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductFeedbackEndpoints.GetProductFeedback),
            new
            {
                productId = productId.ToString(),
            }
        );

        var response = await _client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ProductFeedbackDetailedResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedProductFeedback, _seedUser);
    }

    private IReadOnlyList<DomainProductFeedback> GetProductFeedbackItems(
        ProductId productId
    )
    {
        return _seedProductFeedback.ListAll(f => f.ProductId == productId);
    }
}
