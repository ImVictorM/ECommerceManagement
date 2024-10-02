using Domain.Common.Models;
using FluentAssertions;

namespace Domain.UnitTests.Common.Models;

/// <summary>
/// Tests for the <see cref="Domain.Common.Models.AggregateRoot{TId}"/> model.
/// </summary>
public class AggregateRootTests
{
    private class TestAggregateRoot: AggregateRoot<int>
    {
        public TestAggregateRoot() : base(1)
        {

        }
    }

    /// <summary>
    /// Tests if it is possible to instance an implementation of the <see cref="Domain.Common.Models.AggregateRoot{TId}"/> correctly.
    /// </summary>
    [Fact]
    public void AggregateRoot_WhenImplementedAndCreated_ReturnsNewInstance()
    {
        var ar = new TestAggregateRoot();

        ar.Should().NotBeNull();
        ar.Id.Should().Be(1);
    }
}
