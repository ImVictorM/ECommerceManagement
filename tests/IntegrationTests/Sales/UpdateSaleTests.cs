using Contracts.Sales;

using WebApi.Sales;

using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Sales;
using IntegrationTests.Common;
using IntegrationTests.Sales.TestUtils;
using IntegrationTests.TestUtils.Contracts;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Assertions;

using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;

namespace IntegrationTests.Sales;

/// <summary>
/// Integration tests for the update sale feature.
/// </summary>
public class UpdateSaleTests : BaseIntegrationTest
{
    private readonly ISaleSeed _seedSale;
    private readonly ICategorySeed _seedCategory;
    private readonly IProductSeed _seedProduct;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateSaleTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedSale = SeedManager.GetSeed<ISaleSeed>();
        _seedCategory = SeedManager.GetSeed<ICategorySeed>();
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to update a
    /// sale without authentication.
    /// </summary>
    [Fact]
    public async Task UpdateSale_WithoutAuthentication_ReturnsUnauthorized()
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.UpdateSale),
            new { id = saleId }
        );

        var request = UpdateSaleRequestUtils.CreateRequest();

        var response = await RequestService.CreateClient().PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to update a sale without the admin role.
    /// </summary>
    /// <param name="customerType">
    /// The customer without the admin role type.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task UpdateSale_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.UpdateSale),
            new { id = saleId }
        );

        var request = UpdateSaleRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(customerType);
        var response = await client.PutAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies updating a non-existent sale returns not found.
    /// </summary>
    [Fact]
    public async Task UpdateSale_WithNonExistentSale_ReturnsNotFound()
    {
        var nonExistentSaleId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.UpdateSale),
            new { id = nonExistentSaleId }
        );

        var request = UpdateSaleRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PutAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies it's possible to update a sale with admin authentication.
    /// </summary>
    [Fact]
    public async Task UpdateSale_WithAdminAuthentication_ReturnsNoContent()
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.UpdateSale),
            new { id = saleId }
        );

        var endpointGetById = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.GetSaleById),
            new { id = saleId }
        );

        var request = UpdateSaleRequestUtils.CreateRequest(
            discount: DiscountContractUtils.CreateDiscount(
                percentage: 15,
                description: "Updated discount for tech products",
                startingDate: DateTimeOffset.UtcNow.AddDays(2),
                endingDate: DateTimeOffset.UtcNow.AddDays(20)
            ),
            categoryOnSaleIds:
            [
                _seedCategory.GetEntityId(CategorySeedType.TECHNOLOGY).ToString()
            ],
            productOnSaleIds: [],
            productExcludedFromSaleIds:
            [
                _seedProduct.GetEntityId(ProductSeedType.COMPUTER_ON_SALE).ToString()
            ]
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseUpdate = await client.PutAsJsonAsync(endpointUpdate, request);

        var responseGetUpdated = await client.GetAsync(endpointGetById);
        var responseGetUpdatedContent = await responseGetUpdated.Content
            .ReadRequiredFromJsonAsync<SaleResponse>();

        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetUpdated.StatusCode.Should().Be(HttpStatusCode.OK);

        responseGetUpdatedContent.Discount
            .Should()
            .BeEquivalentTo(
                request.Discount,
                options => options.ComparingWithDateTimeOffset()
            );

        responseGetUpdatedContent.CategoryOnSaleIds
            .Should()
            .BeEquivalentTo(request.CategoryOnSaleIds);

        responseGetUpdatedContent.ProductOnSaleIds
            .Should()
            .BeEquivalentTo(request.ProductOnSaleIds);

        responseGetUpdatedContent.ProductExcludedFromSaleIds
            .Should()
            .BeEquivalentTo(request.ProductExcludedFromSaleIds);
    }

    /// <summary>
    /// Verifies it's not possible to update a sale with an invalid request.
    /// </summary>
    [Fact]
    public async Task UpdateSale_WithAdminAuthenticationAndInvalidRequest_ReturnsBadRequest()
    {
        var saleId = _seedSale
            .GetEntityId(SaleSeedType.TECH_SALE)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(SaleEndpoints.UpdateSale),
            new { id = saleId }
        );

        var request = UpdateSaleRequestUtils.CreateRequest(
            categoryOnSaleIds: [],
            productOnSaleIds: [],
            productExcludedFromSaleIds: []
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var response = await client.PutAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
