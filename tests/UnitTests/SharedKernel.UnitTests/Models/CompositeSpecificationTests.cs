using SharedKernel.Models;
using SharedKernel.UnitTests.Models.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Unit tests for <see cref="CompositeSpecification{T}"/>.
/// </summary>
public class CompositeSpecificationTests
{
    /// <summary>
    /// Data to test the And method.
    /// </summary>
    public static readonly IEnumerable<object[]> AndMethodTestData =
    [
        [
            EntityUtils.NumericEntity.Create(value: 15),
            CompositeSpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            CompositeSpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            true
        ],

        [
            EntityUtils.NumericEntity.Create(value: 5),
            CompositeSpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            CompositeSpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            false
        ],

        [
            EntityUtils.NumericEntity.Create(value : 25),
            CompositeSpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            CompositeSpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            false,
        ]
    ];

    /// <summary>
    /// Verifies that chaining specifications using the
    /// <see cref="CompositeSpecification{T}.And"/> method returns a combined
    /// specification that evaluates both conditions.
    /// </summary>
    [Theory]
    [MemberData(nameof(AndMethodTestData))]
    public void And_WithTwoDifferentSpecifications_ShouldCombineThemCorrectly(
        EntityUtils.NumericEntity<int> entity,
        CompositeSpecification<EntityUtils.NumericEntity<int>> leftSpec,
        CompositeSpecification<EntityUtils.NumericEntity<int>> rightSpec,
        bool expectedCombinationResult
    )
    {
        var combinedSpec = leftSpec.And(rightSpec);

        combinedSpec.IsSatisfiedBy(entity).Should().Be(expectedCombinationResult);
    }
}

