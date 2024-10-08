using Domain.UnitTests.Common.Models.TestUtils;
using FluentAssertions;

namespace Domain.UnitTests.Common.Models;

/// <summary>
/// Tests for the <see cref="Domain.Common.Models.Entity{TId}"/> model.
/// </summary>
public class EntityTests
{

    /// <summary>
    /// A list of ids.
    /// </summary>
    public static IEnumerable<object[]> Ids
    {
        get
        {
            yield return new object[] { 1 };
            yield return new object[] { "1" };
            yield return new object[] { Guid.NewGuid() };
        }
    }

    /// <summary>
    /// A list of pairs of different ids.
    /// </summary>
    public static IEnumerable<object[]> IdsDifferentPairs
    {
        get
        {
            yield return new object[] { 1, "1" };
            yield return new object[] { "1", "5" };
            yield return new object[] { Guid.NewGuid(), 7 };
        }
    }

    /// <summary>
    /// Tests that comparing two entities with the same ID returns true for equality.
    /// </summary>
    /// <param name="id">The ID used to create both entities.</param>
    [Theory]
    [MemberData(nameof(Ids))]
    public void Entity_WhenComparedWithSameId_ShouldReturnTrue(object id)
    {
        var entity1 = EntityUtils.UserEntity.Create(id);
        var entity2 = EntityUtils.UserEntity.Create(id);

        entity1.Equals(entity2).Should().BeTrue();
        (entity1 == entity2).Should().BeTrue();
    }

    /// <summary>
    /// Tests that comparing two entities with different IDs returns false for equality.
    /// </summary>
    /// <param name="id1">The ID of the first entity.</param>
    /// <param name="id2">The ID of the second entity.</param>
    [Theory]
    [MemberData(nameof(IdsDifferentPairs))]
    public void Entity_WhenComparedWithDifferentId_ShouldReturnFalse(object id1, object id2)
    {
        var entity1 = EntityUtils.UserEntity.Create(id1);
        var entity2 = EntityUtils.UserEntity.Create(id2);

        entity1.Equals(entity2).Should().BeFalse();
        (entity1 == entity2).Should().BeFalse();
    }

    /// <summary>
    /// Tests that comparing an entity with a different type (but same ID) returns false for equality.
    /// </summary>
    /// <param name="id">The ID used to create the entity.</param>
    [Theory]
    [MemberData(nameof(Ids))]
    public void Entity_WhenComparedWithDifferentTypeAndSameId_ShouldReturnFalse(object id)
    {
        var userEntity = EntityUtils.UserEntity.Create(id);
        var productEntity = EntityUtils.ProductEntity.Create(id);

        userEntity.Equals(productEntity).Should().BeFalse();
    }

    /// <summary>
    /// Tests that two entities with the same ID have the same hash code.
    /// </summary>
    /// <param name="id">The ID used to create both entities.</param>
    [Theory]
    [MemberData(nameof(Ids))]
    public void Entity_WhenComparedHashCodeWithSameId_ShouldReturnTrue(object id)
    {
        var entity1 = EntityUtils.UserEntity.Create(id);
        var entity2 = EntityUtils.UserEntity.Create(id);

        entity1.GetHashCode().Should().Be(entity2.GetHashCode());
    }

    /// <summary>
    /// Tests that two entities with different IDs have different hash codes.
    /// </summary>
    /// <param name="id1">The ID of the first entity.</param>
    /// <param name="id2">The ID of the second entity.</param>
    [Theory]
    [MemberData(nameof(IdsDifferentPairs))]
    public void Entity_WhenComparedHashCodeWithDifferentId_ShouldReturnFalse(object id1, object id2)
    {
        var entity1 = EntityUtils.UserEntity.Create(id1);
        var entity2 = EntityUtils.UserEntity.Create(id2);

        entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
    }

    /// <summary>
    /// Tests that adding a domain event increases the count of domain events in the entity.
    /// </summary>
    [Fact]
    public void Entity_AddingDomainEvent_IncreasesCount()
    {
        var userEntity = EntityUtils.UserEntity.Create(1);

        userEntity.AddDomainEvent(EntityUtils.DummyDomainEvent.Create());
        userEntity.AddDomainEvent(EntityUtils.DummyDomainEvent.Create());

        userEntity.DomainEvents.Count.Should().Be(2);
    }

    /// <summary>
    /// Tests that clearing domain events resets the count of domain events in the entity to zero.
    /// </summary>
    [Fact]
    public void Entity_ClearingDomainEvents_ShouldResetCount()
    {
        var initialDomainEventsCount = 10;

        var userEntity = EntityUtils.UserEntity.Create(1, initialDomainEventsCount);

        userEntity.DomainEvents.Count.Should().Be(initialDomainEventsCount);

        userEntity.ClearDomainEvents();

        userEntity.DomainEvents.Count.Should().Be(0);
    }
}
