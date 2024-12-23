using SharedKernel.Specifications;
using SharedKernel.UnitTests.Specifications.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveEntityByIdSpecification{TEntity, TEntityId}"/> class.
/// </summary>
public class QueryActiveEntityByIdSpecificationTests
{
    /// <summary>
    /// Verifies that the specification's criteria matches an active entity with the specified ID.
    /// </summary>
    [Fact]
    public void QueryActiveEntityByIdSpecification_WhenEntityIsActiveAndIdMatches_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var entity = ActivableEntityUtils.CreateActivableEntity(id, isActive: true);
        var specification = new QueryActiveEntityByIdSpecification<ActivableEntityUtils.ActivableEntity, Guid>(id);
        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification's criteria does not match an inactive entity with the specified ID.
    /// </summary>
    [Fact]
    public void QueryActiveEntityByIdSpecification_WhenEntityIsInactiveAndIdMatches_ReturnsFalse()
    {
        var id = Guid.NewGuid();
        var entity = ActivableEntityUtils.CreateActivableEntity(id, isActive: false);
        var specification = new QueryActiveEntityByIdSpecification<ActivableEntityUtils.ActivableEntity, Guid>(id);
        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification's criteria does not match an active entity with a different ID.
    /// </summary>
    [Fact]
    public void QueryActiveEntityByIdSpecification_WhenEntityIsActiveAndIdDoesNotMatch_ReturnsFalse()
    {
        var id = Guid.NewGuid();
        var differentId = Guid.NewGuid();
        var entity = ActivableEntityUtils.CreateActivableEntity(differentId, isActive: true);
        var specification = new QueryActiveEntityByIdSpecification<ActivableEntityUtils.ActivableEntity, Guid>(id);
        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification filters a collection to include only the active entity with the specified ID.
    /// </summary>
    [Fact]
    public void QueryActiveEntityByIdSpecification_WhenAppliedToCollection_FiltersActiveEntityById()
    {
        var id = Guid.NewGuid();
        var activeMatchingEntity = ActivableEntityUtils.CreateActivableEntity(id, isActive: true);
        var activeNonMatchingEntity = ActivableEntityUtils.CreateActivableEntity(isActive: true);
        var inactiveMatchingEntity = ActivableEntityUtils.CreateActivableEntity(id, isActive: false);

        var collection = new List<ActivableEntityUtils.ActivableEntity>
        {
            activeMatchingEntity,
            activeNonMatchingEntity,
            inactiveMatchingEntity
        };

        var specification = new QueryActiveEntityByIdSpecification<ActivableEntityUtils.ActivableEntity, Guid>(id);
        var criteria = specification.Criteria;

        var filteredCollection = collection.AsQueryable().Where(criteria).ToList();

        filteredCollection.Should().ContainSingle().Which.Should().Be(activeMatchingEntity);
    }
}
