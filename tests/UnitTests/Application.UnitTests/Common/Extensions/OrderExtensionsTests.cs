using Application.Common.Extensions;
using Domain.OrderAggregate.Enumerations;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Application.UnitTests.Common.Extensions;

/// <summary>
/// Unit tests for the <see cref="OrderExtensions"/> class.
/// </summary>
public class OrderExtensionsTests
{
    /// <summary>
    /// Retrieves all the available order statuses.
    /// </summary>
    public static IEnumerable<object[]> AvailableOrderStatuses()
    {
        foreach (var status in OrderStatus.List())
        {
            yield return new object[] { status };
        }
    }

    /// <summary>
    /// Tests the <see cref="OrderExtensions.GetStatusDescription(Domain.OrderAggregate.Order)"/> method works correctly
    /// by returning the status name.
    /// </summary>
    /// <param name="status">The current order status.</param>
    [Theory]
    [MemberData(nameof(AvailableOrderStatuses))]
    public async Task GetStatusDescription_WhenCalled_ReturnsStatusName(
        OrderStatus status
    )
    {
        var order = await OrderUtils.CreateOrderAsync(initialOrderStatus: status);

        var description = order.GetStatusDescription();

        description.Should().Be(status.Name);
    }
}
