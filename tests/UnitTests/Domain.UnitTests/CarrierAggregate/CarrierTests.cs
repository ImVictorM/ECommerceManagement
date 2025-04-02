using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.CarrierAggregate;

/// <summary>
/// Unit tests for the <see cref="Domain.CarrierAggregate.Carrier"/> class.
/// </summary>
public class CarrierTests
{
    /// <summary>
    /// Provides a list containing valid carrier creation parameters.
    /// </summary>
    public static readonly IEnumerable<object[]> CarrierValidCreationParameters =
    [
        ["FastExpress"],
        ["ReliableShipping"]
    ];

    /// <summary>
    /// Verifies that a carrier can be created successfully.
    /// </summary>
    [Theory]
    [MemberData(nameof(CarrierValidCreationParameters))]
    public void Create_WithValidParameters_CreatesWithoutThrowing(
        string name
    )
    {
        var actionResult = FluentActions
            .Invoking(() => CarrierUtils.CreateCarrier(name: name))
            .Should()
            .NotThrow();

        var carrier = actionResult.Subject;

        carrier.Name.Should().Be(name);
    }
}
