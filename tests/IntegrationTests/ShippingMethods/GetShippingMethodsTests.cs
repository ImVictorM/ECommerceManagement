using Contracts.ShippingMethods;

using Domain.ShippingMethodAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.ShippingMethods;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the get shipping methods feature.
/// </summary>
public class GetShippingMethodsTests : BaseIntegrationTest
{
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetShippingMethodsTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(ShippingMethodEndpoints.GetShippingMethods)
        );
    }

    /// <summary>
    /// Verifies an OK response containing all shipping methods is returned
    /// when requested.
    /// </summary>
    [Fact]
    public async Task GetShippingMethods_WithValidRequest_ReturnsOk()
    {
        var expectedShippingMethods = _seedShippingMethod
            .ListAll()
            .Select(s => new ShippingMethodResponse(
                s.Id.ToString(),
                s.Name,
                s.Price,
                s.EstimatedDeliveryDays
            ));

        var response = await RequestService.Client.GetAsync(_endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<ShippingMethodResponse>>();

        responseContent.Should().BeEquivalentTo(expectedShippingMethods);
    }
}
