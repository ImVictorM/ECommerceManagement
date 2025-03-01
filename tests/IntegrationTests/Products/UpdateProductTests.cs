using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Products.TestUtils;

using WebApi.Products;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the update product feature.
/// </summary>
public class UpdateProductTests : BaseIntegrationTest
{
    private readonly ICategorySeed _seedCategory;
    private readonly IProductSeed _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateProductTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<ICategorySeed>();
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
    }

    /// <summary>
    /// Verifies when an unauthenticated user tries to update a product
    /// the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_WithoutAuthentication_ReturnsUnauthorized()
    {
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.UpdateProduct),
            new { id = "1" }
        );

        var response = await RequestService.CreateClient().PutAsJsonAsync(
            endpoint,
            UpdateProductRequestUtils.CreateRequest()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies when a user that is not an admin tries to update a product
    /// it is returned a forbidden response.
    /// </summary>
    /// <param name="customerUserType">
    /// The customer type to be authenticated.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.CUSTOMER)]
    public async Task UpdateProduct_WithNonAdminUser_ReturnsForbidden(
        UserSeedType customerUserType
    )
    {
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.UpdateProduct),
            new { id = "1" }
        );

        var client = await RequestService.LoginAsAsync(customerUserType);

        var response = await client.PutAsJsonAsync(
            endpoint,
            UpdateProductRequestUtils.CreateRequest()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies when the product to be updated does not exist the response
    /// is not found.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.UpdateProduct),
            new { id = notFoundId }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PutAsJsonAsync(
            endpoint,
            UpdateProductRequestUtils.CreateRequest()
        );
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be(
            $"The product with id {notFoundId} could not be" +
            $" updated because it does not exist"
        );
    }

    /// <summary>
    /// Verifies updating a product with admin permission and valid request
    /// parameters updates the product and returns a no content response.
    /// Also, after updating the product, fetches and tests it
    /// to be sure if it was updated.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_WithAdminRoleAndValidRequest_ReturnsNoContent()
    {
        var productCategories = new[]
        {
            _seedCategory.GetEntity(CategorySeedType.BOOKS_STATIONERY),
            _seedCategory.GetEntity(CategorySeedType.TECHNOLOGY)
        };

        var productCategoryIds = productCategories.Select(c => c.Id.ToString());
        var productCategoryNames = productCategories.Select(c => c.Name);

        var idProductToUpdate = _seedProduct
            .GetEntityId(ProductSeedType.PENCIL)
            .ToString();

        var request = UpdateProductRequestUtils.CreateRequest(
            name: "Techy pen",
            categoryIds: productCategoryIds,
            description: "New tech pen coming in",
            basePrice: 150m,
            images: [new Uri("tech-pencil.png", UriKind.Relative)]
        );

        var endpointUpdate = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.UpdateProduct),
            new { id = idProductToUpdate }
        );

        var endpointGetProductById = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.GetProductById),
            new { id = idProductToUpdate }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var putResponse = await client.PutAsJsonAsync(
            endpointUpdate,
            request
        );
        var getResponse = await client.GetAsync(
            endpointGetProductById
        );
        var getResponseContent = await getResponse.Content
            .ReadRequiredFromJsonAsync<ProductResponse>();

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponseContent.Name.Should().Be(request.Name);
        getResponseContent.Description.Should().Be(request.Description);
        getResponseContent.Images.Should().BeEquivalentTo(request.Images);
        getResponseContent.BasePrice.Should().Be(request.BasePrice);
        getResponseContent.Categories
            .Should()
            .BeEquivalentTo(productCategoryNames);
    }
}
