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
/// Integration tests for the product creation, covering authentication and authorization scenarios,
/// request validation, and expected responses when attempting to create products under different conditions.
/// </summary>
public class CreateProductTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateProductTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Provides invalid product creation requests along with their expected validation error responses.
    /// </summary>
    public static IEnumerable<object[]> InvalidRequests()
    {
        foreach (var (invalidName, expectedErrors) in ProductUtils.GetInvalidNameWithCorrespondingErrors())
        {
            yield return new object[] { CreateProductRequestUtils.CreateRequest(name: invalidName), expectedErrors };
        }

        foreach (var (invalidDescription, expectedErrors) in ProductUtils.GetInvalidDescriptionWithCorrespondingErrors())
        {
            yield return new object[] { CreateProductRequestUtils.CreateRequest(description: invalidDescription), expectedErrors };
        }

        foreach (var (invalidPrice, expectedErrors) in ProductUtils.GetInvalidInitialPriceWithCorrespondingErrors())
        {
            yield return new object[] { CreateProductRequestUtils.CreateRequest(initialPrice: invalidPrice), expectedErrors };
        }

        foreach (var (invalidInitialQuantity, expectedErrors) in ProductUtils.GetInvalidInitialQuantityWithCorrespondingErrors())
        {
            yield return new object[] { CreateProductRequestUtils.CreateRequest(initialQuantity: invalidInitialQuantity), expectedErrors };
        }

        foreach (var (invalidCategories, expectedErrors) in ProductUtils.GetInvalidCategoriesWithCorrespondingErrors())
        {
            yield return new object[] { CreateProductRequestUtils.CreateRequest(categories: invalidCategories), expectedErrors };
        }

        foreach (var (invalidImages, expectedErrors) in ProductUtils.GetInvalidImagesWithCorrespondingErrors())
        {
            yield return new object[] { CreateProductRequestUtils.CreateRequest(images: invalidImages), expectedErrors };
        }
    }

    /// <summary>
    /// Tests that when an authenticated user without admin privileges tries to create a product, the response
    /// status is Forbidden. Ensures that only admins are allowed to create products.
    /// </summary>
    /// <param name="customerType">The type of non-admin user attempting to create a product.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    [InlineData(SeedAvailableUsers.Customer)]
    public async Task CreateProduct_WhenUserAuthenticatedIsNotAdmin_RetunsForbidden(SeedAvailableUsers customerType)
    {
        var request = CreateProductRequestUtils.CreateRequest();

        await Client.LoginAs(customerType);
        var response = await Client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an unauthenticated user attempts to create a product, the response status is Unauthorized.
    /// Ensures that only authenticated users with the required admin role can create products.
    /// </summary>
    [Fact]
    public async Task CreateProduct_WhenUserIsNotAuthenticated_ReturnsUnauthorize()
    {
        var request = CreateProductRequestUtils.CreateRequest();

        var response = await Client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when an authenticated admin user creates a product, the product is created successfully and the
    /// response status is Created. Confirms that admins have permission to create products.
    /// </summary>
    [Fact]
    public async Task CreateProduct_WhenUserAuthenticatedIsAdmin_CreatesProductAndReturnsCreated()
    {
        // TODO: Query for the product and test if it was created correctly (not possible yet)
        var request = CreateProductRequestUtils.CreateRequest();

        await Client.LoginAs(SeedAvailableUsers.Admin);

        var response = await Client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Tests that when an admin user tries to create a product with invalid request parameters, the response
    /// status is BadRequest and includes the expected validation errors. Ensures the validity of request data.
    /// </summary>
    /// <param name="request">The invalid product creation request.</param>
    /// <param name="expectedErrors">Expected validation errors.</param>
    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task CreateProduct_WhenUserAuthenticatedIsAdminButRequestParametersAreInvalid_ReturnsBadRequest(
        CreateProductRequest request,
        Dictionary<string, string[]> expectedErrors
    )
    {
        await Client.LoginAs(SeedAvailableUsers.Admin);
        var httpResponse = await Client.PostAsJsonAsync("/products", request);

        var responseContent = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent!.EnsureCorrespondsToExpectedErrors(expectedErrors);
    }
}
