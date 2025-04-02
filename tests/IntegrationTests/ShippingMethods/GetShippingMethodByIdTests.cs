using Contracts.ShippingMethods;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.ShippingMethods;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the get shipping method by id feature.
/// </summary>
public class GetShippingMethodByIdTests : BaseIntegrationTest
{
    private readonly IShippingMethodSeed _seedShippingMethod;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodByIdTests"/>
    /// class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetShippingMethodByIdTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<IShippingMethodSeed>();

        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies getting a shipping method that does not exist returns not found.
    /// </summary>
    [Fact]
    public async Task GetShippingMethodById_WhenShippingMethodDoesNotExist_ReturnsNotFound()
    {
        var missingShippingMethodId = "241112";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.GetShippingMethodById),
            new { id = missingShippingMethodId }
        );

        var response = await _client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies getting an existent shipping method returns ok containing the
    /// shipping method.
    /// </summary>
    [Fact]
    public async Task GetShippingMethodById_WhenShippingMethodExists_ReturnsOk()
    {
        var existentShippingMethod = _seedShippingMethod.GetEntity(
            ShippingMethodSeedType.EXPRESS
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.GetShippingMethodById),
            new { id = existentShippingMethod.Id.ToString() }
        );

        var response = await _client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ShippingMethodResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        responseContent.Id.Should().Be(existentShippingMethod.Id.ToString());
        responseContent.Name.Should().Be(existentShippingMethod.Name);
        responseContent.Price.Should().Be(existentShippingMethod.Price);
        responseContent.EstimatedDeliveryDays
            .Should()
            .Be(existentShippingMethod.EstimatedDeliveryDays);
    }
}
