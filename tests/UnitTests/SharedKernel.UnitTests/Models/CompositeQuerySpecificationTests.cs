using SharedKernel.Models;
using SharedKernel.UnitTests.Models.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Unit tests for the <see cref="CompositeQuerySpecification{T}"/> class.
/// </summary>
public class CompositeQuerySpecificationTests
{
    /// <summary>
    /// Data to test the And method.
    /// </summary>
    public static readonly IEnumerable<object[]> AndMethodTestData =
    [
        [
            EntityUtils.NumericEntity.Create(value: 15),
            CompositeQuerySpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            CompositeQuerySpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            true
        ],

        [
            EntityUtils.NumericEntity.Create(value : 5),
            CompositeQuerySpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            CompositeQuerySpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            false
        ],

        [
            EntityUtils.NumericEntity.Create(value: 25),
            CompositeQuerySpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            CompositeQuerySpecificationUtils
                .CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            false
        ]
    ];

    /// <summary>
    /// Verifies the <see cref="CompositeQuerySpecification{T}.And"/> method
    /// correctly combines specifications.
    /// </summary>
    [Theory]
    [MemberData(nameof(AndMethodTestData))]
    public void And_WithTwoDifferentSpecifications_ShouldCombineThemCorrectly(
        EntityUtils.NumericEntity<int> entity,
        CompositeQuerySpecification<EntityUtils.NumericEntity<int>> leftSpec,
        CompositeQuerySpecification<EntityUtils.NumericEntity<int>> rightSpec,
        bool expectedCombinationResult
    )
    {
        var compositeSpec = leftSpec.And(rightSpec);

        var result = compositeSpec.Criteria.Compile()(entity);

        result.Should().Be(expectedCombinationResult);
    }
}
