using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.UnitTests.Models.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Unit tests for the <see cref="AndQuerySpecification{T}"/> class.
/// </summary>
public class AndQuerySpecificationTests
{
    /// <summary>
    /// Test data for the AndQuerySpecification.
    /// </summary>
    public static readonly IEnumerable<object[]> AndQueryTestData =
    [
        [
            EntityUtils.NumericEntity.Create(value : 15),
            SpecificationQueryUtils
                .CreateSpecificationQuery<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            SpecificationQueryUtils
                .CreateSpecificationQuery<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            true
        ],

        [
            EntityUtils.NumericEntity.Create(value: 5),
            SpecificationQueryUtils
                .CreateSpecificationQuery<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            SpecificationQueryUtils
                .CreateSpecificationQuery<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            false
        ],

        [
            EntityUtils.NumericEntity.Create(value : 25),
            SpecificationQueryUtils
                .CreateSpecificationQuery<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            SpecificationQueryUtils
                .CreateSpecificationQuery<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            false
        ]
    ];

    /// <summary>
    /// Verifies the <see cref="AndQuerySpecification{T}.Criteria"/> correctly
    /// combines the expressions using AND logic.
    /// </summary>
    [Theory]
    [MemberData(nameof(AndQueryTestData))]
    public void AndQuerySpecification_WithTwoSpecifications_ShouldCombineThemCorrectly(
        EntityUtils.NumericEntity<int> entity,
        ISpecificationQuery<EntityUtils.NumericEntity<int>> leftSpec,
        ISpecificationQuery<EntityUtils.NumericEntity<int>> rightSpec,
        bool expectedCombinationResult
    )
    {
        var combinedSpec = AndQuerySpecificationUtils.CreateSpecification(
            leftSpec,
            rightSpec
        );

        var result = combinedSpec.Criteria.Compile()(entity);

        result.Should().Be(expectedCombinationResult);
    }
}

