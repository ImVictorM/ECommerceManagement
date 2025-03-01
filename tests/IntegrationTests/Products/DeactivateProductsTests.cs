using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Products;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Net;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the deactivate product feature.
/// </summary>
public class DeactivateProductsTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateProductsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeactivateProductsTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
    }

    /// <summary>
    /// Verifies when the user is not authenticated the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_WithoutAuthentication_ReturnsUnauthorized()
    {
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.DeactivateProduct),
            new { id = "1" }
        );

        var response = await RequestService.CreateClient().DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies that when a customer tries to delete a product the response
    /// is forbidden.
    /// </summary>
    /// <param name="customerType">The customer to be authenticated.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    public async Task DeactivateProduct_WhenUserIsNotAdmin_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.DeactivateProduct),
            new { id = "1" }
        );

        var client = await RequestService.LoginAsAsync(customerType);
        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies that when the product to be deactivate does not exist
    /// an not found error is returned.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_WhenProductDoesNotExist_ReturnNotFound()
    {
        var notFoundId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.DeactivateProduct),
            new { id = notFoundId }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.DeleteAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be(
            $"Product with id {notFoundId} could not be deactivated because" +
            $" it does not exist or is already inactive"
        );
    }

    /// <summary>
    /// Verifies when the product is deactivate the response is no content.
    /// Also checks if the product was made inaccessible by trying to fetch it.
    /// </summary>
    /// <param name="productToBeDeactivatedType">
    /// The product to be deactivated type.
    /// </param>
    [Theory]
    [InlineData(ProductSeedType.PENCIL)]
    [InlineData(ProductSeedType.COMPUTER_ON_SALE)]
    [InlineData(ProductSeedType.TSHIRT)]
    public async Task DeactivateProduct_WhenProductExistsAndUserHasPermission_ReturnsNoContent(
        ProductSeedType productToBeDeactivatedType
    )
    {
        var idProductToBeDeactivate = _seedProduct
            .GetEntityId(productToBeDeactivatedType)
            .ToString();

        var endpointDeactivate = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.DeactivateProduct),
            new { id = idProductToBeDeactivate }
        );
        var endpointGetProductById = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.GetProductById),
            new { id = idProductToBeDeactivate }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var responseDelete = await client.DeleteAsync(endpointDeactivate);
        var responseGet = await client.GetAsync(endpointGetProductById);

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGet.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
