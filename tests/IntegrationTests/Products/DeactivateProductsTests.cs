using System.Net;
using System.Net.Http.Json;
using Domain.ProductAggregate;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the process of deactivating a product.
/// </summary>
public class DeactivateProductsTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateProductsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public DeactivateProductsTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// List of products contained in the database.
    /// </summary>
    public static IEnumerable<object[]> SeedProducts()
    {
        foreach (var product in ProductSeed.ListProducts())
        {
            yield return new object[] { product };
        }
    }

    /// <summary>
    /// Tests that when the user is not authenticated the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var response = await Client.DeleteAsync("/products/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when a customer tries to delete a product the response is forbidden.
    /// </summary>
    /// <param name="customerType">The customer to be authenticated.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.Customer)]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    public async Task DeactivateProduct_WhenUserIsNotAdmin_ReturnsForbidden(SeedAvailableUsers customerType)
    {
        await Client.LoginAs(customerType);
        var response = await Client.DeleteAsync("/products/1");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when the product to be deactivate does not exist an not found error is returned.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_WhenProductDoesNotExist_ReturnNotFound()
    {
        var notFoundId = "404";

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var response = await Client.DeleteAsync($"/products/{notFoundId}");
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"Product with id {notFoundId} could not be deactivated because it does not exist");
    }

    /// <summary>
    /// Tests that when the product is deactivate the response is no content.
    /// Also checks if the product was indeed deactivated and the inventory was set to zero.
    /// </summary>
    /// <param name="productToDeactivate">The product to be deactivated.</param>
    [Theory]
    [MemberData(nameof(SeedProducts))]
    public async Task DeactivateProduct_WhenProductExistAndUserHasPermission_DeactivatesAndSetsInventoryToZero(
        Product productToDeactivate
    )
    {
        /// TODO: query for the product and check if it is deactivated.
        await Client.LoginAs(SeedAvailableUsers.Admin);
        var response = await Client.DeleteAsync($"/products/{productToDeactivate.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
