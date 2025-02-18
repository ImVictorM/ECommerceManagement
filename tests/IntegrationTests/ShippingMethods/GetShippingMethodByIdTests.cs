using Domain.ShippingMethodAggregate;

using Contracts.ShippingMethods;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.ShippingMethods;

/// <summary>
/// Integration tests for the process of getting a shipping method by id.
/// </summary>
public class GetShippingMethodByIdTests : BaseIntegrationTest
{
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _seedShippingMethod;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetShippingMethodByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedShippingMethod = SeedManager.GetSeed<ShippingMethodSeedType, ShippingMethod>();
    }

    /// <summary>
    /// Verifies getting a shipping method that does not exist returns not found.
    /// </summary>
    [Fact]
    public async Task GetShippingMethodById_WhenShippingMethodDoesNotExist_ReturnsNotFound()
    {
        var missingShippingMethodId = "241112";
        var endpoint = TestConstants.ShippingMethodEndpoints.GetShippingMethodById(missingShippingMethodId);

        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies getting an existing shipping method returns ok containing the shipping method.
    /// </summary>
    [Fact]
    public async Task GetShippingMethodById_WhenShippingMethodExists_ReturnsOkContainingShippingMethod()
    {
        var existingShippingMethod = _seedShippingMethod.GetByType(ShippingMethodSeedType.EXPRESS);
        var endpoint = TestConstants.ShippingMethodEndpoints.GetShippingMethodById(existingShippingMethod.Id.ToString());

        var response = await RequestService.Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<ShippingMethodResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        responseContent.Id.Should().Be(existingShippingMethod.Id.ToString());
        responseContent.Name.Should().Be(existingShippingMethod.Name);
        responseContent.Price.Should().Be(existingShippingMethod.Price);
        responseContent.EstimatedDeliveryDays.Should().Be(existingShippingMethod.EstimatedDeliveryDays);
    }
}
