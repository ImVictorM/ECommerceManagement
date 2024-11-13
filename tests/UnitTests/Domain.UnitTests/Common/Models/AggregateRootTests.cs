using Domain.UnitTests.Common.Models.TestUtils;
using FluentAssertions;
using SharedKernel.Models;

namespace Domain.UnitTests.Common.Models;

/// <summary>
/// Tests for the <see cref="AggregateRoot{TId}"/> model.
/// </summary>
public class AggregateRootTests
{

    /// <summary>
    /// Tests if it is possible to instance an implementation of the <see cref="AggregateRoot{TId}"/> correctly.
    /// </summary>
    [Fact]
    public void AggregateRoot_WhenImplementedAndCreated_ReturnsNewInstance()
    {
        var id = Guid.NewGuid();

        var ar = AggregateRootUtils.UserAggregateRoot.Create(id);

        ar.Should().NotBeNull();
        ar.Id.Should().Be(id);
    }
}
