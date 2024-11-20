using System.Net;
using System.Net.Http.Json;
using Contracts.Products;
using Domain.UnitTests.TestUtils;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.Products.TestUtils;
using IntegrationTests.TestUtils.Extensions.Errors;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
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
    /// Provides invalid product update requests along with their expected validation error responses.
    /// </summary>
    public static IEnumerable<object[]> InvalidRequests()
    {
        foreach (var (invalidName, expectedErrors) in ProductUtils.GetInvalidNameWithCorrespondingErrors())
        {
            yield return new object[] { UpdateProductRequestUtils.CreateRequest(name: invalidName), expectedErrors };
        }

        foreach (var (invalidDescription, expectedErrors) in ProductUtils.GetInvalidDescriptionWithCorrespondingErrors())
        {
            yield return new object[] { UpdateProductRequestUtils.CreateRequest(description: invalidDescription), expectedErrors };
        }

        foreach (var (invalidPrice, expectedErrors) in ProductUtils.GetInvalidBasePriceWithCorrespondingErrors())
        {
            yield return new object[] { UpdateProductRequestUtils.CreateRequest(basePrice: invalidPrice), expectedErrors };
        }

        foreach (var (invalidCategories, expectedErrors) in ProductUtils.GetInvalidCategoriesWithCorrespondingErrors())
        {
            yield return new object[] { UpdateProductRequestUtils.CreateRequest(categories: invalidCategories), expectedErrors };
        }

        foreach (var (invalidImages, expectedErrors) in ProductUtils.GetInvalidImagesWithCorrespondingErrors())
        {
            yield return new object[] { UpdateProductRequestUtils.CreateRequest(images: invalidImages), expectedErrors };
        }
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
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    [InlineData(SeedAvailableUsers.Customer)]
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
        await Client.LoginAs(SeedAvailableUsers.Admin);

        var response = await Client.PutAsJsonAsync($"/products/{notFoundId}", UpdateProductRequestUtils.CreateRequest());
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"The product with id {notFoundId} could not be updated because it does not exist");
    }

    /// <summary>
    /// Tests that when the request is invalid the response is bad request.
    /// </summary>
    /// <param name="request">The invalid request.</param>
    /// <param name="expectedErrors">The expected errors.</param>
    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task UpdateProduct_WhenRequestHasInvalidData_ReturnsBadRequest(
        UpdateProductRequest request,
        Dictionary<string, string[]> expectedErrors
    )
    {
        await Client.LoginAs(SeedAvailableUsers.Admin);
        var httpResponse = await Client.PutAsJsonAsync("/products/1", request);

        var responseContent = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent!.EnsureCorrespondsToExpectedErrors(expectedErrors);
    }

    /// <summary>
    /// Tests when updating the product with right credentials and request parameters the product is updated successfully returning a no content response.
    /// Also, after updating the product, fetches and tests it to be sure if it was updated.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateProduct_WhenUserIsAdminAndRequestIsValid_UpdatesProductAndReturnsNoContent()
    {
        var productToUpdate = ProductSeed.GetSeedProduct(SeedAvailableProducts.PENCIL);
        var request = UpdateProductRequestUtils.CreateRequest();

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var putResponse = await Client.PutAsJsonAsync($"/products/{productToUpdate.Id}", request);
        var getResponse = await Client.GetAsync($"/products/{productToUpdate.Id}");
        var getResponseContent = await getResponse.Content.ReadFromJsonAsync<ProductResponse>();

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponseContent!.Name.Should().Be(request.Name);
        getResponseContent.Categories.Should().BeEquivalentTo(request.Categories);
        getResponseContent.Description.Should().Be(request.Description);
        getResponseContent.Images.Should().BeEquivalentTo(request.Images);
        getResponseContent.OriginalPrice.Should().Be(request.BasePrice);
    }
}
