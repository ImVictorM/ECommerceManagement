using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.UnitTests.Models.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Unit tests for the <see cref="AndSpecification{T}"/> class.
/// </summary>
public class AndSpecificationTests
{
    /// <summary>
    /// Test data for the and specification.
    /// </summary>
    public static readonly IEnumerable<object[]> TestData =
    [
        [
            EntityUtils.NumericEntity.Create(value : 15),
            SpecificationUtils
                .CreateSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            SpecificationUtils
                .CreateSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            true
        ],
        [
            EntityUtils.NumericEntity.Create(value: 5),
            SpecificationUtils
                .CreateSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            SpecificationUtils
                .CreateSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            false,
        ],
        [
            EntityUtils.NumericEntity.Create(value : 25),
            SpecificationUtils
                .CreateSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value > 10
                ),
            SpecificationUtils
                .CreateSpecification<EntityUtils.NumericEntity<int>>(
                    e => e.Value < 20
                ),
            false,
        ]
    ];

    /// <summary>
    /// Verifies the <see cref="AndSpecification{T}.IsSatisfiedBy"/> method returns
    /// the expected result.
    /// </summary>
    [Theory]
    [MemberData(nameof(TestData))]
    public void AndSpecification_WithTwoSpecifications_ShouldCombineThemCorrectly(
        EntityUtils.NumericEntity<int> entity,
        ISpecification<EntityUtils.NumericEntity<int>> leftSpec,
        ISpecification<EntityUtils.NumericEntity<int>> rightSpec,
        bool expectedResult
    )
    {
        var andSpec = AndSpecificationUtils.CreateSpec(leftSpec, rightSpec);

        var result = andSpec.IsSatisfiedBy(entity);

        result.Should().Be(expectedResult);
    }
}
