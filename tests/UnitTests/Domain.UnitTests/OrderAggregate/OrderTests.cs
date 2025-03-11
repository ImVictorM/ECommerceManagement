using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.Errors;
using Domain.OrderAggregate.Factories;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;

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
    /// Verifies the order is created correctly with valid parameters.
    /// </summary>
    [Fact]
    public async Task CreateOrder_WithValidParameters_CreatesWithoutThrowing()
    {
        var ownerId = UserId.Create(1);
        var orderLineItemDrafts = OrderUtils.CreateOrderLineItemDrafts(4);
        var orderLineItems = orderLineItemDrafts
            .Select(d => OrderUtils.CreateOrderLineItem(
                productId: d.ProductId,
                quantity: d.Quantity
            ))
            .ToList();

        var total = orderLineItems.Sum(op => op.CalculateTransactionPrice());

        var mockOrderAssemblyService = new Mock<IOrderAssemblyService>();
        var mockOrderPricingService = new Mock<IOrderPricingService>();

        mockOrderAssemblyService
            .Setup(s => s.AssembleOrderLineItemsAsync(
                orderLineItemDrafts,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(orderLineItems);

        mockOrderPricingService
            .Setup(s => s.CalculateTotalAsync(
                orderLineItems,
                It.IsAny<ShippingMethodId>(),
                It.IsAny<IEnumerable<OrderCoupon>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(total);

        var factory = new OrderFactory(
            mockOrderAssemblyService.Object,
            mockOrderPricingService.Object
        );

        var actionResult = await FluentActions
            .Invoking(() => factory.CreateOrderAsync(
                requestId: Guid.NewGuid(),
                ownerId: UserId.Create(1),
                shippingMethodId: ShippingMethodId.Create(2),
                orderLineItemDrafts: orderLineItemDrafts,
                paymentMethod: OrderUtils.CreateMockPaymentMethod(),
                billingAddress: AddressUtils.CreateAddress(),
                deliveryAddress: AddressUtils.CreateAddress(),
                installments: 1,
                couponsApplied: [],
                default
            ))
            .Should()
            .NotThrowAsync();

        var order = actionResult.Subject;

        order.OwnerId.Should().Be(ownerId);
        order.Total.Should().Be(total);
        order.Description.Should().Be("Order pending. Waiting for payment");
        order.OrderStatus.Should().Be(OrderStatus.Pending);
        order.DomainEvents.Should().HaveCount(1);
        order.DomainEvents.Should().ContainItemsAssignableTo<OrderCreated>();
        order.OrderTrackingEntries.Should().HaveCount(1);
        order.OrderTrackingEntries.First().OrderStatus.Should().Be(OrderStatus.Pending);
        order.Products.Should().BeEquivalentTo(orderLineItems);
    }

    /// <summary>
    /// Verifies canceling a pending order sets the order status to
    /// <see cref="OrderStatus.Canceled"/>, updates the description to match the
    /// reason, and creates a new order tracking entry.
    /// </summary>
    [Fact]
    public async Task CancelOrder_WithPendingOrder_CompletesSuccessfully()
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
    /// Verifies canceling an order with status different than pending results
    /// in an exception being throwed.
    /// </summary>
    /// <param name="initialStatusDifferentThanPending">
    /// The status different than pending.
    /// </param>
    [Theory]
    [MemberData(nameof(OrderStatusesDifferentThanPending))]
    public async Task CancelOrder_WithoutPendingOrder_ThrowsError(
        OrderStatus initialStatusDifferentThanPending
    )
    {
        var order = await OrderUtils.CreateOrderAsync(
            initialOrderStatus: initialStatusDifferentThanPending
        );

        FluentActions
            .Invoking(() => order.Cancel("Any"))
            .Should()
            .Throw<InvalidOrderCancellationException>();
    }

    /// <summary>
    /// Verifies it is possible to mark a pending order as paid.
    /// </summary>
    [Fact]
    public async Task MarkAsPaid_WithPendingOrder_CompletesSuccessfully()
    {
        var order = await OrderUtils.CreateOrderAsync(
            initialOrderStatus: OrderStatus.Pending
        );

        order.MarkAsPaid();

        order.DomainEvents.Should().ContainItemsAssignableTo<OrderPaid>();
        order.OrderStatus.Should().Be(OrderStatus.Paid);
        order.OrderTrackingEntries.Should().Contain(o => o.OrderStatus == OrderStatus.Paid);
    }

    /// <summary>
    /// Verifies an error is thrown when trying to mark an order as paid when the
    /// order status is different than pending.
    /// </summary>
    /// <param name="statusDifferentThanPending">
    /// The order initial status.
    /// </param>
    [Theory]
    [MemberData(nameof(OrderStatusesDifferentThanPending))]
    public async Task MarkAsPaid_WithoutPendingOrder_ThrowsError(
        OrderStatus statusDifferentThanPending
    )
    {
        var order = await OrderUtils.CreateOrderAsync(
            initialOrderStatus: statusDifferentThanPending
        );

        FluentActions
            .Invoking(order.MarkAsPaid)
            .Should()
            .Throw<InvalidOrderStateForPaymentException>();
    }
}
