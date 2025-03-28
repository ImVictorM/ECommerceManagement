using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ShippingMethodAggregate;

/// <summary>
/// Unit tests for the <see cref="Domain.ShippingMethodAggregate.ShippingMethod"/>
/// class.
/// </summary>
public class ShippingMethodTests
{
    /// <summary>
    /// Provides a list containing valid shipping method creation parameters.
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
    /// Verifies that a shipping method can be created successfully.
    /// </summary>
    [Theory]
    [MemberData(nameof(ShippingMethodValidCreationParameters))]
    public void Create_WithValidParameters_CreatesWithoutThrowing(
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

    /// <summary>
    /// Verifies it is possible to completely update a shipping method.
    /// </summary>
    [Fact]
    public void Update_WithCompleteUpdate_UpdatesTheShippingMethodCompletely()
    {
        var newName = "SuperFastShipping";
        var newPrice = 50m;
        var newEstimatedDeliveryDays = 1;

        var shippingMethod = ShippingMethodUtils.CreateShippingMethod();

        shippingMethod.Update(
            name: newName,
            price: newPrice,
            estimatedDeliveryDays: newEstimatedDeliveryDays
        );

        shippingMethod.Name.Should().Be(newName);
        shippingMethod.Price.Should().Be(newPrice);
        shippingMethod.EstimatedDeliveryDays.Should().Be(newEstimatedDeliveryDays);
    }

    /// <summary>
    /// Verifies it is possible to partially update a shipping method.
    /// </summary>
    [Fact]
    public void Update_WithPartialUpdate_UpdatesTheShippingMethodPartially()
    {
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod();

        var newName = "SuperFastShipping";
        var initialEstimatedDeliveryDays = shippingMethod.EstimatedDeliveryDays;
        var initialPrice = shippingMethod.Price;

        shippingMethod.Update(name: newName);

        shippingMethod.Name.Should().Be(newName);
        shippingMethod.Price.Should().Be(initialPrice);
        shippingMethod.EstimatedDeliveryDays
            .Should().Be(initialEstimatedDeliveryDays);
    }
}
