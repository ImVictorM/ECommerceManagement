using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.OrderAggregate.Services;

using FluentAssertions;
using Moq;

namespace Domain.UnitTests.OrderAggregate;

/// <summary>
/// Unit tests for the <see cref="Order"/> class.
/// </summary>
public class OrderTests
{
    /// <summary>
    /// Tests the order is created correctly when creating it with correct parameters.
    /// </summary>
    [Fact]
    public async Task CreateOrder_WithValidParameters_CreatesWithoutThrowing()
    {
        var mockReservedProducts = OrderUtils.CreateReservedProducts(2);

        var mockOrderProducts = mockReservedProducts.Select(
            (input) => OrderUtils.CreateOrderProduct(input.ProductId, input.Quantity)
        ).ToHashSet();

        var mockTotal = mockOrderProducts.Sum(op => op.CalculateTransactionPrice());

        var mockOrderService = new Mock<IOrderService>();

        mockOrderService
            .Setup(s => s.PrepareOrderProductsAsync(mockReservedProducts))
            .Returns(mockOrderProducts.ToAsyncEnumerable());

        mockOrderService
            .Setup(s => s.CalculateTotalAsync(mockOrderProducts, It.IsAny<IEnumerable<OrderCoupon>>()))
            .ReturnsAsync(mockTotal);

        var actionResult = await FluentActions
            .Invoking(() => OrderUtils.CreateOrderAsync(
                orderProducts: mockReservedProducts,
                installments: 1,
                orderService: mockOrderService.Object
            ))
            .Should()
            .NotThrowAsync();

        var order = actionResult.Subject;

        order.OwnerId.Should().NotBeNull();
        order.Total.Should().Be(mockTotal);
        order.Description.Should().Be("Order pending. Waiting for payment");
        order.OrderStatusId.Should().Be(OrderStatus.Pending.Id);
        order.DomainEvents.Should().HaveCount(1);
        order.DomainEvents.Should().ContainItemsAssignableTo<OrderCreated>();
        order.OrderStatusHistories.Should().HaveCount(1);
        order.OrderStatusHistories.First().OrderStatusId.Should().Be(OrderStatus.Pending.Id);
        order.Products.Should().BeEquivalentTo(mockOrderProducts);
    }

    /// <summary>
    /// Tests that canceling an order sets the order status to canceled, updates the description to match the reason, and increments the order status history.
    /// </summary>
    [Fact]
    public async Task CancelOrder_WhenOrderIsPending_UpdateFieldsCorrectly()
    {
        var reasonToCancel = "Important reason";
        var order = await OrderUtils.CreateOrderAsync();

        order.CancelOrder(reasonToCancel);

        order.OrderStatusId.Should().Be(OrderStatus.Canceled.Id);
        order.Description.Should().Be(reasonToCancel);
        order.OrderStatusHistories.Count.Should().Be(2);
        order.OrderStatusHistories.Should().Contain(osh => osh.OrderStatusId == OrderStatus.Canceled.Id);
    }

    /// <summary>
    /// Tests that getting the order status description returns correctly.
    /// </summary>
    [Fact]
    public async Task GetOrderStatusDescription_WhenOrderIsPending_ReturnsPendingName()
    {
        var order = await OrderUtils.CreateOrderAsync();

        order.GetStatusDescription().Should().Be(OrderStatus.Pending.Name);
    }
}
