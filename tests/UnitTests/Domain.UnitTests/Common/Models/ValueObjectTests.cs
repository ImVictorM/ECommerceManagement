using Domain.UnitTests.Common.TestUtils;
using FluentAssertions;

namespace Domain.UnitTests.Common.Models;

/// <summary>
/// Tests for the <see cref="Domain.Common.Models.ValueObject"/> model.
/// </summary>
public class ValueObjectTests
{
    /// <summary>
    /// Compare two Address value objects with the same properties. They should be considered equal.
    /// </summary>
    [Theory]
    [InlineData("Limeira", "São Paulo")]
    [InlineData("New York", "NY")]
    public void ValueObject_WhenComparedWithSameProperties_ShouldReturnTrue(string city, string state)
    {
        var obj1 = ValueObjectUtils.CreateAddressValueObject(city, state);
        var obj2 = ValueObjectUtils.CreateAddressValueObject(city, state);

        obj1.Equals(obj2).Should().BeTrue();
        (obj1 == obj2).Should().BeTrue();
    }

    /// <summary>
    /// Compare two objects of the same type with different values. They should not be considered equal.
    /// </summary>
    [Fact]
    public void ValueObject_WhenComparedWithDifferentProperties_ShouldReturnFalse()
    {
        var obj1 = ValueObjectUtils.CreateAddressValueObject("Limeira", "São Paulo");
        var obj2 = ValueObjectUtils.CreateAddressValueObject("New York", "NY");

        obj1.Equals(obj2).Should().BeFalse();
        (obj1 == obj2).Should().BeFalse();
    }

    /// <summary>
    /// Compare the hash code of two objects of the same type with equal values. The comparison should equal to true.
    /// </summary>
    /// <param name="city">The city of the address.</param>
    /// <param name="state">The state of the address.</param>
    [Theory]
    [InlineData("Limeira", "São Paulo")]
    [InlineData("New York", "NY")]
    public void ValueObject_WhenComparedHashCodeOfEqualValueObjects_ShouldReturnTrue(string city, string state)
    {
        var obj1 = ValueObjectUtils.CreateAddressValueObject(city, state);
        var obj2 = ValueObjectUtils.CreateAddressValueObject(city, state);

        obj1.GetHashCode().Should().Be(obj2.GetHashCode());
    }

    /// <summary>
    /// Compare the hash code of two objects of the same type with equal values. The comparison should equal to true.
    /// </summary>
    [Fact]
    public void ValueObject_WhenComparedHashCodeOfDifferentValueObjects_ShouldReturnFalse()
    {
        var obj1 = ValueObjectUtils.CreateAddressValueObject("Limeira", "São Paulo");
        var obj2 = ValueObjectUtils.CreateAddressValueObject("New York", "NY");

        obj1.GetHashCode().Should().NotBe(obj2.GetHashCode());
    }

    /// <summary>
    /// Compare two objects of different types with the same properties and values. The result should be false.
    /// </summary>
    [Fact]
    public void ValueObject_WhenComparedWithDifferentTypeAndSameValue_ShouldReturnFalse()
    {
        var value = "email@email.com";

        var email = ValueObjectUtils.CreateEmailValueObject(value);
        var password = ValueObjectUtils.CreatePasswordValueObject(value);

        email.Equals(password).Should().BeFalse();
        (email == password).Should().BeFalse();
    }
}
