using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ShippingMethodAggregate;

/// <summary>
/// Unit tests for the <see cref="Domain.ShippingMethodAggregate.ShippingMethod"/> class.
/// </summary>
public class ShippingMethodTests
{
    /// <summary>
    /// List containing valid shipping method creation parameters.
    /// </summary>
    public static readonly IEnumerable<object[]> ShippingMethodValidCreationParameters =
    [
        [
            "FreeShipment",
            0m,
            12
        ],
        [
            "Express",
            25m,
            5
        ]
    ];

    /// <summary>
    /// Tests that a shipping method can be created successfully.
    /// </summary>
    [Theory]
    [MemberData(nameof(ShippingMethodValidCreationParameters))]
    public void CreateShippingMethod_WithValidParameters_CreatesWithoutThrowing(
        string name,
        decimal price,
        int estimatedDeliveryDays
    )
    {
        var actionResult = FluentActions
            .Invoking(() => ShippingMethodUtils.CreateShippingMethod(
                name: name,
                price: price,
                estimatedDeliveryDays: estimatedDeliveryDays
            ))
            .Should()
            .NotThrow();

        var shippingMethod = actionResult.Subject;

        shippingMethod.Name.Should().NotBeNullOrWhiteSpace();
        shippingMethod.Price.Should().BeGreaterThanOrEqualTo(0);
        shippingMethod.EstimatedDeliveryDays.Should().BeGreaterThan(0);
    }
}
