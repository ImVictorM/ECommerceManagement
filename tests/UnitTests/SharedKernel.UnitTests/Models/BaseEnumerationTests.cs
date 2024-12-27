using FluentAssertions;
using SharedKernel.Models;
using static SharedKernel.UnitTests.Models.TestUtils.BaseEnumerationUtils;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Unit tests for the <see cref="BaseEnumeration"/> class, verifying its behavior for equality,
/// comparison, retrieval, and utility methods.
/// </summary>
public class BaseEnumerationTests
{
    /// <summary>
    /// Tests that two enumeration instances with the same id and type are considered equal.
    /// </summary>
    [Fact]
    public void EnumerationEquality_WithSameIdAndType_ReturnsTrue()
    {
        var enum1 = new SampleEnumeration(1, "First");
        var enum2 = SampleEnumeration.First;

        enum1.Should().Be(enum2);
    }

    /// <summary>
    /// Tests that two enumeration instances with different ids are not considered equal.
    /// </summary>
    [Fact]
    public void EnumerationEquality_WithDifferentId_ReturnsFalse()
    {
        var enum1 = new SampleEnumeration(1, "First");
        var enum2 = SampleEnumeration.Second;

        enum1.Should().NotBe(enum2);
    }

    /// <summary>
    /// Tests that comparing two enumeration instances with the same id returns zero.
    /// </summary>
    [Fact]
    public void CompareEnumeration_WithSameId_ReturnsZero()
    {
        var enum1 = new SampleEnumeration(1, "First");
        var enum2 = SampleEnumeration.First;

        enum1.CompareTo(enum2).Should().Be(0);
    }

    /// <summary>
    /// Tests that comparing an enumeration instance with a lower id to one with a higher id returns a positive value.
    /// </summary>
    [Fact]
    public void CompareEnumeration_WithLowerId_ReturnsPositive()
    {
        var enum1 = SampleEnumeration.Second;
        var enum2 = SampleEnumeration.First;

        enum1.CompareTo(enum2).Should().BeGreaterThan(0);
    }

    /// <summary>
    /// Tests that comparing an enumeration instance with a higher id to one with a lower id returns a negative value.
    /// </summary>
    [Fact]
    public void CompareEnumeration_WithHigherId_ReturnsNegative()
    {
        var enum1 = SampleEnumeration.First;
        var enum2 = SampleEnumeration.Second;

        enum1.CompareTo(enum2).Should().BeLessThan(0);
    }

    /// <summary>
    /// Tests that <see cref="BaseEnumeration.GetAll{T}"/> retrieves all defined enumeration instances of a type.
    /// </summary>
    [Fact]
    public void GetAll_WhenCalledWithType_ReturnsAllDefinedInstances()
    {
        var allInstances = BaseEnumeration.GetAll<SampleEnumeration>().ToList();

        allInstances.Should().Contain(SampleEnumeration.First);
        allInstances.Should().Contain(SampleEnumeration.Second);
        allInstances.Count.Should().Be(2);
    }

    /// <summary>
    /// Tests that <see cref="BaseEnumeration.FromValue{T}"/> retrieves the correct enumeration instance for a valid id.
    /// </summary>
    [Fact]
    public void FromValue_WithValidId_ReturnsCorrectInstance()
    {
        var result = BaseEnumeration.FromValue<SampleEnumeration>(1);

        SampleEnumeration.First.Should().Be(result);
    }

    /// <summary>
    /// Tests that <see cref="BaseEnumeration.FromValue{T}"/> throws an exception for an invalid id.
    /// </summary>
    [Fact]
    public void FromValue_WithInvalidId_ThrowsException()
    {
        FluentActions
            .Invoking(() => BaseEnumeration.FromValue<SampleEnumeration>(3))
            .Should()
            .Throw<InvalidOperationException>();
    }

    /// <summary>
    /// Tests that <see cref="BaseEnumeration.FromDisplayName{T}"/> retrieves the correct enumeration instance for a valid display name.
    /// </summary>
    [Fact]
    public void FromDisplayName_WithValidName_ReturnsCorrectInstance()
    {
        var result = BaseEnumeration.FromDisplayName<SampleEnumeration>("Second");

        SampleEnumeration.Second.Should().Be(result);
    }

    /// <summary>
    /// Tests that <see cref="BaseEnumeration.FromDisplayName{T}"/> throws an exception for an invalid display name.
    /// </summary>
    [Fact]
    public void FromDisplayName_WithInvalidName_ThrowsException()
    {
        FluentActions
            .Invoking(() => BaseEnumeration.FromDisplayName<SampleEnumeration>("Invalid"))
            .Should()
            .Throw<InvalidOperationException>();
    }

    /// <summary>
    /// Tests that <see cref="BaseEnumeration.AbsoluteDifference"/> calculates the correct absolute difference between two enumeration instances.
    /// </summary>
    [Fact]
    public void EnumerationDifference_WhenCalculatingAbsoluteDifference_ReturnsCorrectDifference()
    {
        var result = BaseEnumeration.AbsoluteDifference(SampleEnumeration.First, SampleEnumeration.Second);

        result.Should().Be(1);
    }
}
