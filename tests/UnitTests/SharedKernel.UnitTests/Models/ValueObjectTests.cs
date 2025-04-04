using SharedKernel.UnitTests.Models.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Tests for the <see cref="SharedKernel.Models.ValueObject"/> model.
/// </summary>
public class ValueObjectTests
{
    /// <summary>
    /// Compare two Address value objects with the same properties.
    /// They should be considered equal.
    /// </summary>
    [Theory]
    [InlineData("Limeira", "São Paulo")]
    [InlineData("New York", "NY")]
    public void CompareValueObject_WithSameProperties_ReturnsTrue(
        string city,
        string state
    )
    {
        var obj1 = ValueObjectUtils.AddressValueObject.Create(city, state);
        var obj2 = ValueObjectUtils.AddressValueObject.Create(city, state);

        obj1.Equals(obj2).Should().BeTrue();
        (obj1 == obj2).Should().BeTrue();
    }

    /// <summary>
    /// Compare two objects of the same type with different values.
    /// They should not be considered equal.
    /// </summary>
    [Fact]
    public void CompareValueObject_WithDifferentProperties_ReturnsFalse()
    {
        var obj1 = ValueObjectUtils.AddressValueObject.Create(
            "Limeira",
            "São Paulo"
        );
        var obj2 = ValueObjectUtils.AddressValueObject.Create(
            "New York",
            "NY"
        );

        obj1.Equals(obj2).Should().BeFalse();
        (obj1 == obj2).Should().BeFalse();
    }

    /// <summary>
    /// Compare the hash code of two objects of the same type with equal values.
    /// The comparison should equal to true.
    /// </summary>
    /// <param name="city">The city of the address.</param>
    /// <param name="state">The state of the address.</param>
    [Theory]
    [InlineData("Limeira", "São Paulo")]
    [InlineData("New York", "NY")]
    public void CompareValueObject_WhenTheyAreEqual_TheHashCodesAreEqual(
        string city,
        string state
    )
    {
        var obj1 = ValueObjectUtils.AddressValueObject.Create(city, state);
        var obj2 = ValueObjectUtils.AddressValueObject.Create(city, state);

        obj1.GetHashCode().Should().Be(obj2.GetHashCode());
    }

    /// <summary>
    /// Compare the hash code of two objects of the same type with equal values.
    /// The comparison should equal to true.
    /// </summary>
    [Fact]
    public void CompareValueObject_WhenTheyAreNotEqual_TheHashCodesAreNotEqual()
    {
        var obj1 = ValueObjectUtils.AddressValueObject.Create(
            "Limeira",
            "São Paulo"
        );
        var obj2 = ValueObjectUtils.AddressValueObject.Create("New York", "NY");

        obj1.GetHashCode().Should().NotBe(obj2.GetHashCode());
    }

    /// <summary>
    /// Compare two objects of different types with the same properties and values.
    /// The result should be false.
    /// </summary>
    [Fact]
    public void CompareValueObject_WithDifferentTypeAndSameValue_ReturnsFalse()
    {
        var value = "email@email.com";

        var email = ValueObjectUtils.EmailValueObject.Create(value);
        var password = ValueObjectUtils.PasswordValueObject.Create(value);

        email.Equals(password).Should().BeFalse();
        (email == password).Should().BeFalse();
    }
}
