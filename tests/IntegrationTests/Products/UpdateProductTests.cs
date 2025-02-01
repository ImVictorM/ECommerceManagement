using IntegrationTests.Common;
using IntegrationTests.Products.TestUtils;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;

using Contracts.Products;

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
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateProductTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests that when an unauthenticated user tries to update a product the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var response = await Client.PutAsJsonAsync("/products/1", UpdateProductRequestUtils.CreateRequest());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when a user that is not admin tries to update a product it returns forbidden.
    /// </summary>
    /// <param name="customerUserType">The customer type to be authenticated.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.CUSTOMER_WITH_ADDRESS)]
    [InlineData(SeedAvailableUsers.CUSTOMER)]
    public async Task UpdateProduct_WhenUserIsNotAdmin_ReturnsForbidden(SeedAvailableUsers customerUserType)
    {
        await Client.LoginAs(customerUserType);

        var response = await Client.PutAsJsonAsync("/products/1", UpdateProductRequestUtils.CreateRequest());

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when the product to be updated does not exist the response is not found.
    /// </summary>
    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";
        await Client.LoginAs(SeedAvailableUsers.ADMIN);

        var response = await Client.PutAsJsonAsync($"/products/{notFoundId}", UpdateProductRequestUtils.CreateRequest());
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)HttpStatusCode.NotFound);
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
            CategorySeed.GetSeedCategory(SeedAvailableCategories.BOOKS_STATIONERY),
            CategorySeed.GetSeedCategory(SeedAvailableCategories.TECHNOLOGY)
        };

        var productToUpdate = ProductSeed.GetSeedProduct(SeedAvailableProducts.PENCIL);

        var request = UpdateProductRequestUtils.CreateRequest(
            name: "Techy pen",
            categoryIds: productCategories.Select(c => c.Id.ToString()),
            description: "New tech pen coming in",
            basePrice: 150m,
            images: [new Uri("tech-pencil.png", UriKind.Relative)]
        );

        await Client.LoginAs(SeedAvailableUsers.ADMIN);
        var putResponse = await Client.PutAsJsonAsync($"/products/{productToUpdate.Id}", request);
        var getResponse = await Client.GetAsync($"/products/{productToUpdate.Id}");
        var getResponseContent = await getResponse.Content.ReadFromJsonAsync<ProductResponse>();

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponseContent!.Name.Should().Be(request.Name);
        getResponseContent.Description.Should().Be(request.Description);
        getResponseContent.Images.Should().BeEquivalentTo(request.Images);
        getResponseContent.BasePrice.Should().Be(request.BasePrice);
        getResponseContent.Categories.Should().BeEquivalentTo(productCategories.Select(c => c.Name));
    }
}
