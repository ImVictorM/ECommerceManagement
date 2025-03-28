using SharedKernel.Errors;
using SharedKernel.Models;
using static SharedKernel.UnitTests.Models.TestUtils.BaseEnumerationUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Unit tests for the <see cref="BaseEnumeration"/> class.
/// </summary>
public class BaseEnumerationTests
{
    /// <summary>
    /// Verifies that two enumeration instances with the same identifier and type
    /// are considered equal.
    /// </summary>
    [Fact]
    public void EnumerationEquality_WithSameIdAndType_ReturnsTrue()
    {
        var enum1 = SampleEnumeration.First;
        var enum2 = new SampleEnumeration(enum1.Id, enum1.Name);

        enum1.Should().Be(enum2);
    }

    /// <summary>
    /// Verifies that two enumeration instances with different identifiers
    /// are not considered equal.
    /// </summary>
    [Fact]
    public void EnumerationEquality_WithDifferentId_ReturnsFalse()
    {
        var enum1 = SampleEnumeration.First;
        var enum2 = SampleEnumeration.Second;

        enum1.Should().NotBe(enum2);
    }

    /// <summary>
    /// Verifies that comparing two enumeration instances with the same identifier
    /// returns zero.
    /// </summary>
    [Fact]
    public void CompareEnumeration_WithSameId_ReturnsZero()
    {
        var enum1 = SampleEnumeration.First;
        var enum2 = new SampleEnumeration(enum1.Id, enum1.Name);

        enum1.CompareTo(enum2).Should().Be(0);
    }

    /// <summary>
    /// Verifies that comparing an enumeration instance with a lower identifier
    /// to one with a higher identifier returns a positive value.
    /// </summary>
    [Fact]
    public void CompareEnumeration_WithLowerId_ReturnsPositive()
    {
        var enum1 = SampleEnumeration.Second;
        var enum2 = SampleEnumeration.First;

        enum1.CompareTo(enum2).Should().BeGreaterThan(0);
    }

    /// <summary>
    /// Verifies that comparing an enumeration instance with a higher identifier
    /// to one with a lower identifier returns a negative value.
    /// </summary>
    [Fact]
    public void CompareEnumeration_WithHigherId_ReturnsNegative()
    {
        var enum1 = SampleEnumeration.First;
        var enum2 = SampleEnumeration.Second;

        enum1.CompareTo(enum2).Should().BeLessThan(0);
    }

    /// <summary>
    /// Verifies that <see cref="BaseEnumeration.GetAll{T}"/> retrieves all defined
    /// enumeration instances of a type.
    /// </summary>
    [Fact]
    public void GetAll_WithValidType_ReturnsAllDefinedInstances()
    {
        var allInstances = BaseEnumeration.GetAll<SampleEnumeration>().ToList();

        allInstances.Should().Contain(SampleEnumeration.First);
        allInstances.Should().Contain(SampleEnumeration.Second);
        allInstances.Count.Should().Be(2);
    }

    /// <summary>
    /// Verifies that <see cref="BaseEnumeration.FromValue{T}"/> retrieves the
    /// correct enumeration instance for a valid identifier.
    /// </summary>
    [Fact]
    public void FromValue_WithValidId_ReturnsCorrectInstance()
    {
        var result = BaseEnumeration.FromValue<SampleEnumeration>(1);

        SampleEnumeration.First.Should().Be(result);
    }

    /// <summary>
    /// Verifies that <see cref="BaseEnumeration.FromValue{T}"/> throws an exception
    /// for an invalid identifier.
    /// </summary>
    [Fact]
    public void FromValue_WithInvalidId_ThrowsException()
    {
        FluentActions
            .Invoking(() => BaseEnumeration.FromValue<SampleEnumeration>(3))
            .Should()
            .Throw<InvalidParseException>();
    }

    /// <summary>
    /// Verifies the <see cref="BaseEnumeration.FromDisplayName{T}"/> method
    /// retrieves the correct enumeration instance for a valid display name.
    /// </summary>
    [Fact]
    public void FromDisplayName_WithValidName_ReturnsCorrectInstance()
    {
        var name = SampleEnumeration.Second.Name;

        var result = BaseEnumeration.FromDisplayName<SampleEnumeration>(name);

        SampleEnumeration.Second.Should().Be(result);
    }

    /// <summary>
    /// Verifies the <see cref="BaseEnumeration.FromDisplayName{T}"/> method
    /// throws an exception for an invalid display name.
    /// </summary>
    [Fact]
    public void FromDisplayName_WithInvalidName_ThrowsException()
    {
        FluentActions
            .Invoking(() =>
                BaseEnumeration.FromDisplayName<SampleEnumeration>("Invalid")
            )
            .Should()
            .Throw<InvalidParseException>();
    }

    /// <summary>
    /// Verifies that <see cref="BaseEnumeration.AbsoluteDifference"/> calculates
    /// the correct absolute difference between two enumeration instances.
    /// </summary>
    [Fact]
    public void EnumerationDifference_WithTwoDifferentEnumerations_ReturnsCorrectDifference()
    {
        var result = BaseEnumeration.AbsoluteDifference(
            SampleEnumeration.First,
            SampleEnumeration.Second
        );

        result.Should().Be(1);
    }
}
