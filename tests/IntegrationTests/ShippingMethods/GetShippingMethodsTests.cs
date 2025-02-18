using Contracts.ShippingMethods;

using Domain.ShippingMethodAggregate;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the process of getting all available shipping methods.
/// </summary>
public class GetShippingMethodsTests : BaseIntegrationTest
{
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodsTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetShippingMethodsTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();
    }

    /// <summary>
    /// Verifies it returns all available shipping methods.
    /// </summary>
    [Fact]
    public async Task GetShippingMethods_WithValidRequest_ReturnsOkContainingAllAvailableShippingMethods()
    {
        var expectedShippingMethods = _seedShippingMethod
            .ListAll()
            .Select(s => new ShippingMethodResponse(s.Id.ToString(), s.Name, s.Price, s.EstimatedDeliveryDays));
        var endpoint = TestConstants.ShippingMethodEndpoints.GetShippingMethods;

        var response = await RequestService.Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<ShippingMethodResponse>>();

        responseContent.Should().BeEquivalentTo(expectedShippingMethods);
    }
}
