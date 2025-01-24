using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;

namespace SharedKernel.UnitTests.ValueObjects;

/// <summary>
/// Tests for the <see cref="Address"/> aggregate root.
/// </summary>
public class AddressTests
{
    /// <summary>
    /// List of actions that create addresses with valid parameters.
    /// </summary>
    public static readonly IEnumerable<object[]> ActionsWithValidAddressParameters =
    [
        [
            () => AddressUtils.CreateAddress(postalCode: "66845"),
        ],
        [
            () => AddressUtils.CreateAddress(street: "401 Mill" ),
        ],
        [
            () => AddressUtils.CreateAddress(neighborhood: "Borderlands"),
        ],
        [
            () => AddressUtils.CreateAddress(state: "Kansas"),
        ],
        [
            () => AddressUtils.CreateAddress(city: "Cottonwood Falls"),
        ],
    ];

    /// <summary>
    /// Tests if the address can be instantiated correctly with valid parameters.
    /// </summary>
    [Theory]
    [MemberData(nameof(ActionsWithValidAddressParameters))]
    public void CreateAddress_WithValidParameters_CreatesWithoutThrowing(Func<Address> action)
    {
        var actionResult = FluentActions
            .Invoking(action)
            .Should()
            .NotThrow();

        actionResult.Subject.Should().NotBeNull();
    }
}
