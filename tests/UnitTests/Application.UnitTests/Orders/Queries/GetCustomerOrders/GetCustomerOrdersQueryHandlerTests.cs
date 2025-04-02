using Application.Common.Persistence.Repositories;
using Application.Orders.Queries.GetCustomerOrders;
using Application.UnitTests.Orders.TestUtils.Extensions;
using Application.UnitTests.Orders.Queries.TestUtils;

using Domain.OrderAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.UserAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using Application.UnitTests.Orders.TestUtils.Projections;

namespace Application.UnitTests.Orders.Queries.GetCustomerOrders;

/// <summary>
/// Unit tests for the <see cref="GetCustomerOrdersQueryHandler"/> handler.
/// </summary>
public class GetCustomerOrdersQueryHandlerTests
{
    private readonly GetCustomerOrdersQueryHandler _handler;
    private readonly Mock<IOrderRepository> _mockOrderRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="GetCustomerOrdersQueryHandlerTests"/> class.
    /// </summary>
    public GetCustomerOrdersQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();

        _handler = new GetCustomerOrdersQueryHandler(
            _mockOrderRepository.Object,
            Mock.Of<ILogger<GetCustomerOrdersQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies all the user orders are retrieved without the any filter.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerOrdersQuery_WithValidQuery_ReturnsOrderResults()
    {
        var query = GetCustomerOrdersQueryUtils.CreateQuery();

        var orderOwnerId = UserId.Create(query.UserId);

        var orders = new List<Order>
        {
            await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(1),
                ownerId: orderOwnerId
            ),
            await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(2),
                ownerId: orderOwnerId
            )
        };

        var orderProjections = OrderProjectionUtils.CreateProjections(orders);

        _mockOrderRepository
            .Setup(repo => repo.GetCustomerOrdersAsync(
                orderOwnerId,
                query.Filters,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(orderProjections);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.EnsureCorrespondsTo(orderProjections);
    }
}
