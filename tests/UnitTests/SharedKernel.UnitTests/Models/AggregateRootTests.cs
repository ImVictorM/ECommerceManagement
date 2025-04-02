using SharedKernel.Models;
using SharedKernel.UnitTests.Models.TestUtils;

using FluentAssertions;

namespace SharedKernel.UnitTests.Models;

/// <summary>
/// Tests for the <see cref="AggregateRoot{TId}"/> model.
/// </summary>
public class AggregateRootTests
{

    /// <summary>
    /// Verifies it is possible to instance an implementation of the
    /// <see cref="AggregateRoot{TId}"/> correctly.
    /// </summary>
    [Fact]
    public void CreateAggregateRoot_WithValidId_ReturnsNewInstance()
    {
        var id = Guid.NewGuid();

        var ar = AggregateRootUtils.UserAggregateRoot.Create(id);

        ar.Should().NotBeNull();
        ar.Id.Should().Be(id);
    }
}
