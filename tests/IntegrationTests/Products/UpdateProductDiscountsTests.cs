using System.Net.Http.Json;
using System.Net;
using IntegrationTests.Common;
using IntegrationTests.Products.TestUtils;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using FluentAssertions;
using Contracts.Products.Common;
using IntegrationTests.TestUtils.Contracts;
using Contracts.Products;
using IntegrationTests.TestUtils.Extensions.Assertions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the process of updating a product's discount list.
/// </summary>
public class UpdateProductDiscountsTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductDiscountsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateProductDiscountsTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// List containing invalid discount contract objects.
    /// </summary>
    public static IEnumerable<object[]> InvalidDiscounts()
    {
        var now = DateTimeOffset.UtcNow;

        yield return new object[]
        {
            new List<Discount>()
            {
                ContractDiscountUtils.CreateDiscount(startingDate: now.AddMonths(-1), endingDate: now.AddMonths(1)),
                ContractDiscountUtils.CreateDiscount(startingDate: now.AddHours(-50), endingDate: now.AddHours(1)),
                ContractDiscountUtils.CreateDiscount(startingDate: now.AddDays(5), endingDate: now.AddDays(2))
            }
        };
    }

    /// <summary>
    /// A list of valid pair discounts containing applicable and future discounts.
    /// </summary>
    public static IEnumerable<object[]> ValidDiscounts()
    {
        var now = DateTimeOffset.UtcNow;

        yield return new object[]
        {
            new List<Discount>()
            {
                ContractDiscountUtils.CreateDiscount(percentage: 10),
                ContractDiscountUtils.CreateDiscount(percentage: 5)
            },
            new List<Discount>()
            {
                ContractDiscountUtils.CreateDiscount(percentage: 20, startingDate: now.AddDays(5), endingDate: now.AddDays(19))
            }
        };
    }

    /// <summary>
    /// Tests when the user is not authenticated the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateProductDiscounts_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var request = UpdateProductDiscountsRequestUtils.CreateRequest();

        var response = await Client.PutAsJsonAsync("/products/1/discounts", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests when a user that is not admin tries to update a product's discount list the response is forbidden.
    /// </summary>
    /// <param name="customerUserType">The customer type to be authenticated.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    [InlineData(SeedAvailableUsers.Customer)]
    public async Task UpdateProductDiscounts_WhenUserIsNotAdmin_ReturnsForbidden(SeedAvailableUsers customerUserType)
    {
        var request = UpdateProductDiscountsRequestUtils.CreateRequest();

        await Client.LoginAs(customerUserType);
        var response = await Client.PutAsJsonAsync("/products/1/discounts", request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when the product does not exist the response is not found.
    /// </summary>
    [Fact]
    public async Task UpdateProductDiscounts_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";
        var request = UpdateProductDiscountsRequestUtils.CreateRequest();
        await Client.LoginAs(SeedAvailableUsers.Admin);

        var response = await Client.PutAsJsonAsync($"/products/{notFoundId}/discounts", request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"Product with id {notFoundId} does not exist");
    }

    /// <summary>
    /// Tests when any discount in the request is invalid the response is a bad request.
    /// </summary>
    /// <param name="invalidDiscounts">The invalid discounts.</param>
    [Theory]
    [MemberData(nameof(InvalidDiscounts))]
    public async Task UpdateProductDiscounts_WhenDiscountsAreNotValid_ReturnsBadRequest(IEnumerable<Discount> invalidDiscounts)
    {
        var productWithDiscount = ProductSeed.GetSeedProduct(SeedAvailableProducts.COMPUTER_WITH_DISCOUNTS);

        var request = UpdateProductDiscountsRequestUtils.CreateRequest(
            discounts: invalidDiscounts
        );

        await Client.LoginAs(SeedAvailableUsers.Admin);

        var response = await Client.PutAsJsonAsync($"/products/{productWithDiscount.Id}/discounts", request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent!.Status.Should().Be((int)HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Tests when discounts are valid and user is allowed the product discounts are updated.
    /// </summary>
    /// <param name="applicableDiscounts">Discounts that are applicable now.</param>
    /// <param name="futureDiscounts">Future discounts.</param>
    [Theory]
    [MemberData(nameof(ValidDiscounts))]
    public async Task UpdateProductDiscounts_WhenDiscountsAreValidAndUserIsAllowed_UpdatesTheProductDiscounts(
        IEnumerable<Discount> applicableDiscounts,
        IEnumerable<Discount> futureDiscounts
    )
    {
        var productWithDiscount = ProductSeed.GetSeedProduct(SeedAvailableProducts.COMPUTER_WITH_DISCOUNTS);

        var request = UpdateProductDiscountsRequestUtils.CreateRequest(
            discounts: applicableDiscounts.Concat(futureDiscounts)
        );

        await Client.LoginAs(SeedAvailableUsers.Admin);

        var responsePut = await Client.PutAsJsonAsync($"/products/{productWithDiscount.Id}/discounts", request);
        var responseGet = await Client.GetAsync($"/products/{productWithDiscount.Id}");
        var responseGetContent = await responseGet.Content.ReadFromJsonAsync<ProductResponse>();

        responsePut.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
        responseGetContent!.DiscountsApplied.Should().BeEquivalentTo(applicableDiscounts, options => options.ComparingWithDateTimeOffset());
    }
}
