using Application.Common.Persistence;
using Application.Orders.Queries.GetCustomerOrders;
using Application.UnitTests.Orders.Queries.TestUtils;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.UserAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Unit tests for the <see cref="GetCustomerOrdersQueryHandler"/> handler.
/// </summary>
public class GetCustomerOrdersQueryHandlerTests
{
    private readonly GetCustomerOrdersQueryHandler _handler;
    private readonly Mock<IOrderRepository> _mockOrderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrdersQueryHandlerTests"/> class.
    /// </summary>
    public GetCustomerOrdersQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<ILogger<GetCustomerOrdersQueryHandler>>();

        _handler = new GetCustomerOrdersQueryHandler(
            _mockOrderRepository.Object,
            mockLogger.Object
        );
    }

    /// <summary>
    /// Verifies all the user orders are retrieved without the status filter.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerOrders_WithoutStatusFilter_ReturnsAllOrders()
    {
        var query = GetCustomerOrdersQueryUtils.CreateQuery();

        var orderOwnerId = UserId.Create(query.UserId);

        var orders = new List<Order>
        {
            await OrderUtils.CreateOrderAsync(id: OrderId.Create(1), ownerId: orderOwnerId),
            await OrderUtils.CreateOrderAsync(id: OrderId.Create(2), ownerId: orderOwnerId)
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
    /// Verifies the user orders are retrieved with the status filter.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerOrders_WithStatusFilter_ReturnsFilteredOrders()
    {
        var query = GetCustomerOrdersQueryUtils.CreateQuery(status: OrderStatus.Pending.Name);

        var orderOwnerId = UserId.Create(query.UserId);

        var orders = new List<Order>
        {
            await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(1),
                ownerId: orderOwnerId,
                initialOrderStatus: OrderStatus.Pending
            ),
            await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(2),
                ownerId: orderOwnerId,
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
