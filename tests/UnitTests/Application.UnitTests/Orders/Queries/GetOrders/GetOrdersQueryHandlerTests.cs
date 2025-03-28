using Application.Common.Persistence.Repositories;
using Application.Orders.Queries.GetOrders;
using Application.UnitTests.Orders.Queries.TestUtils;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Queries.GetOrders;

/// <summary>
/// Unit tests for the <see cref="GetOrdersQueryHandler"/> handler.
/// </summary>
public class GetOrdersQueryHandlerTests
{
    private readonly GetOrdersQueryHandler _handler;
    private readonly Mock<IOrderRepository> _mockOrderRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrdersQueryHandlerTests"/> class.
    /// </summary>
    public GetOrdersQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<ILogger<GetOrdersQueryHandler>>();

        _handler = new GetOrdersQueryHandler(_mockOrderRepository.Object, mockLogger.Object);
    }

    /// <summary>
    /// Verifies all orders are retrieved without the status filter.
    /// </summary>
    [Fact]
    public async Task HandleGetOrders_WithoutStatusFilter_ReturnsAllOrders()
    {
        var query = GetOrdersQueryUtils.CreateQuery();

        var orders = new List<Order>
        {
            await OrderUtils.CreateOrderAsync(id: OrderId.Create(1), ownerId: UserId.Create("1")),
            await OrderUtils.CreateOrderAsync(id: OrderId.Create(2), ownerId: UserId.Create("2"))
        };

        _mockOrderRepository
            .Setup(repo => repo.FindSatisfyingAsync(
                It.IsAny<ISpecificationQuery<Order>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(orders);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.Select(r => r.Order).Should().BeEquivalentTo(orders);
    }

    /// <summary>
    /// Verifies orders are retrieved with the status filter.
    /// </summary>
    [Fact]
    public async Task HandleGetOrders_WithStatusFilter_ReturnsFilteredOrders()
    {
        var query = GetOrdersQueryUtils.CreateQuery(status: OrderStatus.Pending.Name);

        var orders = new List<Order>
        {
            await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(1),
                ownerId: UserId.Create("1"),
                initialOrderStatus: OrderStatus.Pending
            ),
            await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(2),
                ownerId: UserId.Create("2"),
                initialOrderStatus: OrderStatus.Paid
            )
        };

        var ordersPending = orders.Where(o => o.OrderStatus == OrderStatus.Pending);

        _mockOrderRepository
            .Setup(repo => repo.FindSatisfyingAsync(
                It.IsAny<ISpecificationQuery<Order>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(ordersPending);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.Select(r => r.Order).Should().BeEquivalentTo(ordersPending);
    }
}
