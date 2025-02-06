using Domain.ProductAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;

using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the process of deactivating a product.
/// </summary>
public class DeactivateProductsTests : BaseIntegrationTest
{
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateProductsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeactivateProductsTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
    }

    /// <summary>
    /// Tests that when the user is not authenticated the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var response = await RequestService.Client.DeleteAsync("/products/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when a customer tries to delete a product the response is forbidden.
    /// </summary>
    /// <param name="customerType">The customer to be authenticated.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task DeactivateProduct_WhenUserIsNotAdmin_ReturnsForbidden(UserSeedType customerType)
    {
        await RequestService.LoginAsAsync(customerType);
        var response = await RequestService.Client.DeleteAsync("/products/1");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when the product to be deactivate does not exist an not found error is returned.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_WhenProductDoesNotExist_ReturnNotFound()
    {
        var notFoundId = "404";

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.DeleteAsync($"/products/{notFoundId}");
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"Product with id {notFoundId} could not be deactivated because it does not exist or is already inactive");
    }

    /// <summary>
    /// Tests that when the product is deactivate the response is no content.
    /// Also checks if the product was made inaccessible by trying to fetch it.
    /// </summary>
    /// <param name="productToBeDeactivatedType">The product to be deactivated type.</param>
    [Theory]
    [InlineData(ProductSeedType.PENCIL)]
    [InlineData(ProductSeedType.COMPUTER_ON_SALE)]
    [InlineData(ProductSeedType.TSHIRT)]
    public async Task DeactivateProduct_WhenProductExistsAndUserHasPermission_DeactivatesAndMakesItInaccessible(
        ProductSeedType productToBeDeactivatedType
    )
    {
        var productToBeDeactivate = _seedProduct.GetByType(productToBeDeactivatedType);

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseDelete = await RequestService.Client.DeleteAsync($"/products/{productToBeDeactivate.Id}");
        var responseGet = await RequestService.Client.GetAsync($"/products/{productToBeDeactivate.Id}");

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGet.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
