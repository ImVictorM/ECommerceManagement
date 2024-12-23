using SharedKernel.Specifications;

using FluentAssertions;
using SharedKernel.UnitTests.Specifications.TestUtils;

namespace SharedKernel.UnitTests.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveSpecification{T}"/> class.
/// </summary>
public class QueryActiveSpecificationTests
{
    /// <summary>
    /// Verifies that the specification's criteria correctly identifies active entities.
    /// </summary>
    [Fact]
    public void QueryActiveSpecification_WhenEntityIsActive_ReturnsTrue()
    {
        var entity = ActivableEntityUtils.CreateActivableEntity(isActive: true);
        var specification = new QueryActiveSpecification<ActivableEntityUtils.ActivableEntity>();
        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification's criteria correctly identifies inactive entities.
    /// </summary>
    [Fact]
    public void QueryActiveSpecification_WhenEntityIsInactive_ReturnsFalse()
    {
        var entity = ActivableEntityUtils.CreateActivableEntity(isActive: false);
        var specification = new QueryActiveSpecification<ActivableEntityUtils.ActivableEntity>();
        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification is composable and can filter a collection to include only active entities.
    /// </summary>
    [Fact]
    public void QueryActiveSpecification_WhenAppliedToCollection_FiltersActiveEntities()
    {
        var activeEntity = ActivableEntityUtils.CreateActivableEntity(isActive: true);
        var inactiveEntity = ActivableEntityUtils.CreateActivableEntity(isActive: false);
        var collection = new List<ActivableEntityUtils.ActivableEntity> { activeEntity, inactiveEntity };

        var specification = new QueryActiveSpecification<ActivableEntityUtils.ActivableEntity>();
        var criteria = specification.Criteria;

        var filteredCollection = collection.AsQueryable().Where(criteria).ToList();

        filteredCollection.Should().ContainSingle().Which.Should().Be(activeEntity);
    }
}
