using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.Specification;

using SharedKernel.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.UserAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryUserByEmailSpecification"/> class.
/// </summary>
public class QueryUserByEmailSpecificationTests
{
    /// <summary>
    /// Verifies that the specification returns true when the user's email matches the provided email.
    /// </summary>
    [Fact]
    public void QueryUserByEmailSpecification_WhenEmailMatches_ReturnsTrue()
    {
        var email = EmailUtils.CreateEmail("test@example.com");
        var user = UserUtils.CreateCustomer(email: email);

        var specification = new QueryUserByEmailSpecification(email);

        var result = specification.Criteria.Compile()(user);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification returns false when the user's email does not match the provided email.
    /// </summary>
    [Fact]
    public void QueryUserByEmailSpecification_WhenEmailDoesNotMatch_ReturnsFalse()
    {
        var email = EmailUtils.CreateEmail("test@example.com");
        var user = UserUtils.CreateCustomer(email: EmailUtils.CreateEmail("other@example.com"));

        var specification = new QueryUserByEmailSpecification(email);

        var result = specification.Criteria.Compile()(user);

        result.Should().BeFalse();
    }
}
