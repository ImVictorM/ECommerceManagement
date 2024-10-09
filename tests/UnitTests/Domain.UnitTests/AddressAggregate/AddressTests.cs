using Domain.AddressAggregate;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using FluentAssertions;

namespace Domain.UnitTests.AddressAggregate;

/// <summary>
/// Tests for the <see cref="Address"/> aggregate root.
/// </summary>
public class AddressTests
{
    /// <summary>
    /// List of valid address parameters.
    /// </summary>
    /// <returns>A list of valid addresses</returns>
    public static IEnumerable<object[]> ValidAddresses()
    {
 
        yield return new object[] {
            "66845",
            TestConstants.Address.Street,
            TestConstants.Address.Neighborhood,
            TestConstants.Address.State,
            TestConstants.Address.City
        };
        yield return new object[] {
            TestConstants.Address.PostalCode,
            "401 Mill",
            TestConstants.Address.Neighborhood,
            TestConstants.Address.State,
            TestConstants.Address.City
        };
        yield return new object[] {
            TestConstants.Address.PostalCode,
            TestConstants.Address.Street,
            "Borderlands",
            TestConstants.Address.State,
            TestConstants.Address.City
        };
        yield return new object[] {
            TestConstants.Address.PostalCode,
            TestConstants.Address.Street,
            TestConstants.Address.Neighborhood,
            "Kansas",
            TestConstants.Address.City
        };
        yield return new object[] {
            TestConstants.Address.PostalCode,
            TestConstants.Address.Street,
            TestConstants.Address.Neighborhood,
            TestConstants.Address.State,
            "Cottonwood Falls"
        };
    }

    /// <summary>
    /// Tests if the address can be instantiated correctly with valid parameters.
    /// </summary>
    /// <param name="postalCode">The valid address postal code.</param>
    /// <param name="street">The valid address street.</param>
    /// <param name="neighborhood">The valid address neighborhood.</param>
    /// <param name="state">The valid address state.</param>
    /// <param name="city">The valid address city.</param>
    [Theory]
    [MemberData(nameof(ValidAddresses))]
    public void Address_WhenCreatedWithValidParameters_ReturnsNewInstance(
        string postalCode,
        string street,
        string neighborhood,
        string state,
        string city
    )
    {
        var act = () => AddressUtils.CreateAddress(
            postalCode: postalCode,
            street: street,
            neighborhood: neighborhood,
            state: state,
            city: city
        );

        act.Should().NotThrow();
    }
}