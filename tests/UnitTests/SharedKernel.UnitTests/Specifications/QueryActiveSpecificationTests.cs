using SharedKernel.Specifications;
using SharedKernel.UnitTests.Specifications.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveSpecification{T}"/> class.
/// </summary>
public class QueryActiveSpecificationTests
{
    /// <summary>
    /// Verifies that the specification's criteria correctly identifies
    /// active entities.
    /// </summary>
    [Fact]
    public void QueryActiveSpecification_WithActiveEntity_ReturnsTrue()
    {
        var entity = ActivatableEntityUtils.CreateActivatableEntity(isActive: true);
        var specification = new QueryActiveSpecification
            <ActivatableEntityUtils.ActivatableEntity>();

        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification's criteria correctly identifies inactive
    /// entities.
    /// </summary>
    [Fact]
    public void QueryActiveSpecification_WithInactiveEntity_ReturnsFalse()
    {
        var entity = ActivatableEntityUtils.CreateActivatableEntity(isActive: false);

        var specification = new QueryActiveSpecification
            <ActivatableEntityUtils.ActivatableEntity>();

        var criteria = specification.Criteria.Compile();

        var result = criteria(entity);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification is composable and can filter a collection
    /// to include only active entities.
    /// </summary>
    [Fact]
    public void QueryActiveSpecification_WithCollection_FiltersActiveEntities()
    {
        var activeEntity = ActivatableEntityUtils
            .CreateActivatableEntity(isActive: true);
        var inactiveEntity = ActivatableEntityUtils
            .CreateActivatableEntity(isActive: false);

        var collection = new List<ActivatableEntityUtils.ActivatableEntity>
        {
            activeEntity,
            inactiveEntity
        };

        var specification = new QueryActiveSpecification
            <ActivatableEntityUtils.ActivatableEntity>();

        var criteria = specification.Criteria;

        var filteredCollection = collection.AsQueryable().Where(criteria).ToList();

        filteredCollection.Should().ContainSingle().Which.Should().Be(activeEntity);
    }
}
