using Contracts.Common;

using WebApi.Sales;

using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common;
using IntegrationTests.Sales.TestUtils;

using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Sales;

/// <summary>
/// Integration tests for the create sale feature.
/// </summary>
public class CreateSaleTests : BaseIntegrationTest
{
    private readonly string? _endpoint;
    private readonly IProductSeed _productSeed;
    private readonly ICategorySeed _categorySeed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateSaleTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _endpoint = LinkGenerator.GetPathByName(nameof(SaleEndpoints.CreateSale));
        _productSeed = SeedManager.GetSeed<IProductSeed>();
        _categorySeed = SeedManager.GetSeed<ICategorySeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when trying to create a
    /// sale without authentication.
    /// </summary>
    [Fact]
    public async Task CreateSale_WithoutAuthentication_ReturnsUnauthorized()
    {
        var request = CreateSaleRequestUtils.CreateRequest();

        var response = await RequestService.CreateClient().PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to create a sale without the admin role.
    /// </summary>
    /// <param name="customerType">
    /// The customer without the admin role type.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task CreateSale_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var request = CreateSaleRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(customerType);

        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies it's possible to create a sale with admin authentication
    /// and a valid request.
    /// </summary>
    [Fact]
    public async Task CreateSale_WithAdminAuthenticationAndValidRequest_ReturnsCreated()
    {
        var request = CreateSaleRequestUtils.CreateRequest(
            discount: new DiscountContract(
                10,
                "10 percent discount",
                DateTimeOffset.UtcNow.AddMinutes(-120),
                DateTimeOffset.UtcNow.AddDays(30)
            ),
            categoryOnSaleIds:
            [
                _categorySeed.GetEntityId(CategorySeedType.SPORTS).ToString()
            ],
            productOnSaleIds:
            [
                _productSeed.GetEntityId(ProductSeedType.PENCIL).ToString(),
                _productSeed.GetEntityId(ProductSeedType.COMPUTER_ON_SALE).ToString()
            ],
            productExcludedFromSaleIds:
            [
                _productSeed.GetEntityId(ProductSeedType.TSHIRT).ToString(),
            ]
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Verifies it's not possible to create a sale with an invalid request.
    /// </summary>
    [Fact]
    public async Task CreateSale_WithAdminAuthenticationAndInvalidRequest_ReturnsBadRequest()
    {
        var request = CreateSaleRequestUtils.CreateRequest(
            categoryOnSaleIds: [],
            productOnSaleIds: []
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var response = await client.PostAsJsonAsync(_endpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
