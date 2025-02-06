using Domain.CategoryAggregate;
using Domain.ProductAggregate;

using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Products.TestUtils;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the process of updating a product.
/// </summary>
public class UpdateProductTests : BaseIntegrationTest
{
    private readonly IDataSeed<CategorySeedType, Category> _seedCategory;
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateProductTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<CategorySeedType, Category>();
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
    }

    /// <summary>
    /// Tests that when an unauthenticated user tries to update a product the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var response = await RequestService.Client.PutAsJsonAsync("/products/1", UpdateProductRequestUtils.CreateRequest());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when a user that is not admin tries to update a product it returns forbidden.
    /// </summary>
    /// <param name="customerUserType">The customer type to be authenticated.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.CUSTOMER)]
    public async Task UpdateProduct_WhenUserIsNotAdmin_ReturnsForbidden(UserSeedType customerUserType)
    {
        await RequestService.LoginAsAsync(customerUserType);
        var response = await RequestService.Client.PutAsJsonAsync("/products/1", UpdateProductRequestUtils.CreateRequest());

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when the product to be updated does not exist the response is not found.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.PutAsJsonAsync($"/products/{notFoundId}", UpdateProductRequestUtils.CreateRequest());
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"The product with id {notFoundId} could not be updated because it does not exist");
    }

    /// <summary>
    /// Tests when updating the product with right credentials and request parameters the product is updated successfully returning a no content response.
    /// Also, after updating the product, fetches and tests it to be sure if it was updated.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_WhenUserIsAdminAndRequestIsValid_UpdatesProductAndReturnsNoContent()
    {
        var productCategories = new[]
        {
            _seedCategory.GetByType(CategorySeedType.BOOKS_STATIONERY),
            _seedCategory.GetByType(CategorySeedType.TECHNOLOGY)
        };

        var productToUpdate = _seedProduct.GetByType(ProductSeedType.PENCIL);

        var request = UpdateProductRequestUtils.CreateRequest(
            name: "Techy pen",
            categoryIds: productCategories.Select(c => c.Id.ToString()),
            description: "New tech pen coming in",
            basePrice: 150m,
            images: [new Uri("tech-pencil.png", UriKind.Relative)]
        );

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var putResponse = await RequestService.Client.PutAsJsonAsync($"/products/{productToUpdate.Id}", request);
        var getResponse = await RequestService.Client.GetAsync($"/products/{productToUpdate.Id}");
        var getResponseContent = await getResponse.Content.ReadRequiredFromJsonAsync<ProductResponse>();

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponseContent.Name.Should().Be(request.Name);
        getResponseContent.Description.Should().Be(request.Description);
        getResponseContent.Images.Should().BeEquivalentTo(request.Images);
        getResponseContent.BasePrice.Should().Be(request.BasePrice);
        getResponseContent.Categories.Should().BeEquivalentTo(productCategories.Select(c => c.Name));
    }
}
