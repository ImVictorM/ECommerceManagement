using FluentAssertions;
using SharedKernel.Models;
using SharedKernel.UnitTests.Models.TestUtils;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Unit tests for the <see cref="CompositeQuerySpecification{T}"/> class.
/// </summary>
public class CompositeQuerySpecificationTests
{
    /// <summary>
    /// Test data for the CompositeQuerySpecification.
    /// </summary>
    public static readonly IEnumerable<object[]> CompositeQueryTestData =
    [
        [
            EntityUtils.NumericEntity.Create(15),
            CompositeQuerySpecificationUtils.CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(e => e.Value > 10),
            CompositeQuerySpecificationUtils.CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(e => e.Value < 20),
            true
        ],

        [
            EntityUtils.NumericEntity.Create(5),
            CompositeQuerySpecificationUtils.CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(e => e.Value > 10),
            CompositeQuerySpecificationUtils.CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(e => e.Value < 20),
            false
        ],

        [
            EntityUtils.NumericEntity.Create(25),
            CompositeQuerySpecificationUtils.CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(e => e.Value > 10),
            CompositeQuerySpecificationUtils.CreateCompositeSpecification<EntityUtils.NumericEntity<int>>(e => e.Value < 20),
            false
        ]
    ];

    /// <summary>
    /// Tests that the <see cref="CompositeQuerySpecification{T}.And"/> method correctly combines specifications.
    /// </summary>
    [Theory]
    [MemberData(nameof(CompositeQueryTestData))]
    public void CompositeQuerySpecification_WhenUsingAndMethod_CombinesSpecificationsCorrectly(
        EntityUtils.NumericEntity<int> entity,
        CompositeQuerySpecification<EntityUtils.NumericEntity<int>> leftSpec,
        CompositeQuerySpecification<EntityUtils.NumericEntity<int>> rightSpec,
        bool expectedCombinationResult
    )
    {
        var compositeSpec = leftSpec.And(rightSpec);

        var predicate = compositeSpec.Criteria.Compile();
        var result = predicate(entity);

        result.Should().Be(expectedCombinationResult);
    }
}
