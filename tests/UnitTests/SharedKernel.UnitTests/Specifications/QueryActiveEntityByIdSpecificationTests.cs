using SharedKernel.Specifications;
using SharedKernel.UnitTests.Specifications.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Specifications;

/// <summary>
/// Unit tests for the
/// <see cref="QueryActiveEntityByIdSpecification{TEntity, TEntityId}"/> class.
/// </summary>
public class QueryActiveEntityByIdSpecificationTests
{
    /// <summary>
    /// Verifies that the specification's criteria matches an active entity with
    /// the specified identifier.
    /// </summary>
    [Fact]
    public void QueryActiveEntityByIdSpecification_WithActiveAndMatchingId_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var entity = ActivatableEntityUtils.CreateActivatableEntity(
            id,
            isActive: true
        );

        var specification = new QueryActiveEntityByIdSpecification
            <ActivatableEntityUtils.ActivatableEntity, Guid>(id);

        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification's criteria does not match an inactive entity
    /// with the specified identifier.
    /// </summary>
    [Fact]
    public void QueryActiveEntityByIdSpecification_WithInactiveAndMatchingId_ReturnsFalse()
    {
        var id = Guid.NewGuid();
        var entity = ActivatableEntityUtils.CreateActivatableEntity(
            id,
            isActive: false
        );

        var specification = new QueryActiveEntityByIdSpecification
            <ActivatableEntityUtils.ActivatableEntity, Guid>(id);

        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification's criteria does not match an active entity
    /// with a different identifier.
    /// </summary>
    [Fact]
    public void QueryActiveEntityByIdSpecification_WithActiveAndNotMatchingId_ReturnsFalse()
    {
        var id = Guid.NewGuid();
        var differentId = Guid.NewGuid();
        var entity = ActivatableEntityUtils.CreateActivatableEntity(
            differentId,
            isActive: true
        );

        var specification = new QueryActiveEntityByIdSpecification
            <ActivatableEntityUtils.ActivatableEntity, Guid>(id);

        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification filters a collection to include only the
    /// active entity with the specified identifier.
    /// </summary>
    [Fact]
    public void QueryActiveEntityByIdSpecification_WithCollection_FiltersActiveEntityById()
    {
        var id = Guid.NewGuid();
        var activeMatchingEntity = ActivatableEntityUtils.CreateActivatableEntity(
            id,
            isActive: true
        );
        var activeNonMatchingEntity = ActivatableEntityUtils.CreateActivatableEntity(
            isActive: true
        );
        var inactiveMatchingEntity = ActivatableEntityUtils.CreateActivatableEntity(
            id,
            isActive: false
        );

        var collection = new List<ActivatableEntityUtils.ActivatableEntity>
        {
            activeMatchingEntity,
            activeNonMatchingEntity,
            inactiveMatchingEntity
        };

        var specification = new QueryActiveEntityByIdSpecification
            <ActivatableEntityUtils.ActivatableEntity, Guid>(id);

        var criteria = specification.Criteria;

        var filteredCollection = collection
            .AsQueryable()
            .Where(criteria)
            .ToList();

        filteredCollection
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Be(activeMatchingEntity);
    }
}
