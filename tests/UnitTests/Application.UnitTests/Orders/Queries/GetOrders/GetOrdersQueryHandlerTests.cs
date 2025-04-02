using Application.Common.Persistence.Repositories;
using Application.Orders.Queries.GetOrders;
using Application.UnitTests.Orders.Queries.TestUtils;
using Application.UnitTests.Orders.TestUtils.Extensions;

using Domain.OrderAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using Application.UnitTests.Orders.TestUtils.Projections;

namespace Application.UnitTests.Orders.Queries.GetOrders;

/// <summary>
/// Unit tests for the <see cref="GetOrdersQueryHandler"/> handler.
/// </summary>
public class GetOrdersQueryHandlerTests
{
    private readonly GetOrdersQueryHandler _handler;
    private readonly Mock<IOrderRepository> _mockOrderRepository;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetOrdersQueryHandlerTests"/> class.
    /// </summary>
    public GetOrdersQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();

        _handler = new GetOrdersQueryHandler(
            _mockOrderRepository.Object,
            Mock.Of<ILogger<GetOrdersQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies all orders are retrieved without the status filter.
    /// </summary>
    [Fact]
    public async Task HandleGetOrdersQuery_WithValidQuery_ReturnsOrderResults()
    {
        var query = GetOrdersQueryUtils.CreateQuery();

        var orders = new List<Order>
        {
            await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(1),
                ownerId: UserId.Create("1")
            ),
            await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(2),
                ownerId: UserId.Create("2")
            )
        };

        var orderProjections = OrderProjectionUtils.CreateProjections(orders);

        _mockOrderRepository
            .Setup(repo => repo.GetOrdersAsync(
                query.Filters,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(orderProjections);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.EnsureCorrespondsTo(orderProjections);
    }
}
