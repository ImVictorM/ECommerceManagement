using SharedKernel.UnitTests.Models.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Tests for the <see cref="SharedKernel.Models.Entity{TId}"/> model.
/// </summary>
public class EntityTests
{

    /// <summary>
    /// Defines a list of different identifiers.
    /// </summary>
    public static readonly IEnumerable<object[]> Ids =
    [
        [1],
        ["1"],
        [Guid.NewGuid()]
    ];

    /// <summary>
    /// Defines a list of different identifier pairs.
    /// </summary>
    public static readonly IEnumerable<object[]> IdsDifferentPairs =
    [
        [1, "1"],
        ["1", "5"],
        [Guid.NewGuid(), 7]
    ];

    /// <summary>
    /// Verifies that comparing two entities with the same identifier returns
    /// true for equality.
    /// </summary>
    /// <param name="id">The identifier used to create both entities.</param>
    [Theory]
    [MemberData(nameof(Ids))]
    public void CompareEntity_WithSameId_ReturnsTrue(object id)
    {
        var entity1 = EntityUtils.UserEntity.Create(id);
        var entity2 = EntityUtils.UserEntity.Create(id);

        entity1.Equals(entity2).Should().BeTrue();
        (entity1 == entity2).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that comparing two entities with different identifiers returns
    /// false for equality.
    /// </summary>
    /// <param name="id1">The identifier of the first entity.</param>
    /// <param name="id2">The identifier of the second entity.</param>
    [Theory]
    [MemberData(nameof(IdsDifferentPairs))]
    public void CompareEntity_WithDifferentId_ReturnsFalse(object id1, object id2)
    {
        var entity1 = EntityUtils.UserEntity.Create(id1);
        var entity2 = EntityUtils.UserEntity.Create(id2);

        entity1.Equals(entity2).Should().BeFalse();
        (entity1 == entity2).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that comparing an entity with a different type and same
    /// identifier returns false for equality.
    /// </summary>
    /// <param name="id">The identifier used to create the entities.</param>
    [Theory]
    [MemberData(nameof(Ids))]
    public void CompareEntity_WithDifferentTypeAndSameId_ReturnsFalse(object id)
    {
        var userEntity = EntityUtils.UserEntity.Create(id);
        var productEntity = EntityUtils.ProductEntity.Create(id);

        userEntity.Equals(productEntity).Should().BeFalse();
    }

    /// <summary>
    /// Verifies that two entities with the same identifier have the same hash code.
    /// </summary>
    /// <param name="id">The identifier used to create both entities.</param>
    [Theory]
    [MemberData(nameof(Ids))]
    public void CompareEntity_WhenTheyAreEqual_TheHashCodesAreEqual(object id)
    {
        var entity1 = EntityUtils.UserEntity.Create(id);
        var entity2 = EntityUtils.UserEntity.Create(id);

        entity1.GetHashCode().Should().Be(entity2.GetHashCode());
    }

    /// <summary>
    /// Verifies that two entities with different identifiers have different
    /// hash codes.
    /// </summary>
    /// <param name="id1">The identifier of the first entity.</param>
    /// <param name="id2">The identifier of the second entity.</param>
    [Theory]
    [MemberData(nameof(IdsDifferentPairs))]
    public void CompareEntity_WhenTheyAreDifferent_TheHashCodesAreDifferent(
        object id1,
        object id2
    )
    {
        var entity1 = EntityUtils.UserEntity.Create(id1);
        var entity2 = EntityUtils.UserEntity.Create(id2);

        entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
    }

    /// <summary>
    /// Verifies that adding a domain event increases the count of domain events
    /// in the entity.
    /// </summary>
    [Fact]
    public void AddDomainEvent_WithValidDomainEvent_IncreasesCount()
    {
        var userEntity = EntityUtils.UserEntity.Create(1);

        userEntity.AddDomainEvent(EntityUtils.DummyDomainEvent.Create());
        userEntity.AddDomainEvent(EntityUtils.DummyDomainEvent.Create());

        userEntity.DomainEvents.Count.Should().Be(2);
    }

    /// <summary>
    /// Verifies that clearing domain events resets the count of domain events
    /// in the entity to zero.
    /// </summary>
    [Fact]
    public void ClearDomainEvents_WithMultipleEvents_ShouldRemoveAllTheEvents()
    {
        var initialDomainEventsCount = 10;

        var userEntity = EntityUtils.UserEntity.Create(
            1,
            initialDomainEventsCount
        );

        userEntity.DomainEvents.Count.Should().Be(initialDomainEventsCount);

        userEntity.ClearDomainEvents();

        userEntity.DomainEvents.Count.Should().Be(0);
        userEntity.DomainEvents.Should().BeEmpty();
    }
}
