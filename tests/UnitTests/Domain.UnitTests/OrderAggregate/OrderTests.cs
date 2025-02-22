using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.Errors;
using Domain.ShippingMethodAggregate.ValueObjects;

using FluentAssertions;
using Moq;

namespace Domain.UnitTests.OrderAggregate;

/// <summary>
/// Unit tests for the <see cref="Order"/> class.
/// </summary>
public class OrderTests
{
    /// <summary>
    /// List of order status different than pending.
    /// </summary>
    public static IEnumerable<object[]> OrderStatusesDifferentThanPending()
    {
        var orderStatuses = OrderStatus.List().Where(os => os != OrderStatus.Pending);

        foreach (var status in orderStatuses)
        {
            yield return new object[] { status };
        }
    }

    /// <summary>
    /// List of order status.
    /// </summary>
    public static IEnumerable<object[]> OrderStatuses()
    {
        var orderStatuses = OrderStatus.List();

        foreach (var status in orderStatuses)
        {
            yield return new object[] { status };
        }
    }

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
            .Setup(s => s.PrepareOrderProductsAsync(
                mockReservedProducts,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(mockOrderProducts);

        mockOrderService
            .Setup(s => s.CalculateTotalAsync(
                mockOrderProducts,
                It.IsAny<ShippingMethodId>(),
                It.IsAny<IEnumerable<OrderCoupon>>(),
                It.IsAny<CancellationToken>()
            ))
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
        order.OrderStatus.Should().Be(OrderStatus.Pending);
        order.DomainEvents.Should().HaveCount(1);
        order.DomainEvents.Should().ContainItemsAssignableTo<OrderCreated>();
        order.OrderTrackingEntries.Should().HaveCount(1);
        order.OrderTrackingEntries.First().OrderStatus.Should().Be(OrderStatus.Pending);
        order.Products.Should().BeEquivalentTo(mockOrderProducts);
    }

    /// <summary>
    /// Tests that canceling an order sets the order status to canceled, updates the description to match the reason, and increments the order status history.
    /// </summary>
    [Fact]
    public async Task CancelOrder_WhenOrderIsPending_UpdatesItCorrectly()
    {
        var reasonToCancel = "Important reason";
        var order = await OrderUtils.CreateOrderAsync();

        order.Cancel(reasonToCancel);

        order.OrderStatus.Should().Be(OrderStatus.Canceled);
        order.Description.Should().Be(reasonToCancel);
        order.OrderTrackingEntries.Count.Should().Be(2);
        order.OrderTrackingEntries.Should().Contain(o => o.OrderStatus == OrderStatus.Canceled);
        order.DomainEvents.Should().ContainItemsAssignableTo<OrderCanceled>();
    }

    /// <summary>
    /// Verifies that cancelling an order with status different than pending results in an exception
    /// being throwed.
    /// </summary>
    /// <param name="status">The status different than pending.</param>
    [Theory]
    [MemberData(nameof(OrderStatusesDifferentThanPending))]
    public async Task CancelOrder_WhenOrderIsNotPending_ThrowsError(OrderStatus status)
    {
        var order = await OrderUtils.CreateOrderAsync(initialOrderStatus: status);

        FluentActions
            .Invoking(() => order.Cancel("Any"))
            .Should()
            .Throw<InvalidOrderCancellationException>();
    }

    /// <summary>
    /// Tests when order is pending it is possible to mark it as paid.
    /// </summary>
    [Fact]
    public async Task MarkAsPaid_WhenOrderIsPending_UpdatesItCorrectly()
    {
        var order = await OrderUtils.CreateOrderAsync(initialOrderStatus: OrderStatus.Pending);

        order.MarkAsPaid();

        order.DomainEvents.Should().ContainItemsAssignableTo<OrderPaid>();
        order.OrderStatus.Should().Be(OrderStatus.Paid);
        order.OrderTrackingEntries.Should().Contain(o => o.OrderStatus == OrderStatus.Paid);
    }

    /// <summary>
    /// Tests an error is thrown when trying to mark order as paid when the
    /// order status is different than pending.
    /// </summary>
    /// <param name="statusDifferentThanPending">The order initial status.</param>
    [Theory]
    [MemberData(nameof(OrderStatusesDifferentThanPending))]
    public async Task MarkAsPaid_WhenOrderIsNotPending_ThrowsError(OrderStatus statusDifferentThanPending)
    {
        var order = await OrderUtils.CreateOrderAsync(initialOrderStatus: statusDifferentThanPending);

        FluentActions
            .Invoking(order.MarkAsPaid)
            .Should()
            .Throw<InvalidOrderStateForPaymentException>();
    }
}
