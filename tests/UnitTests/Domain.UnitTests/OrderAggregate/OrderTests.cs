using Domain.CategoryAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;

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
    /// Tests the order is created correctly when creating it with correct parameters.
    /// </summary>
    [Fact]
    public async Task CreateOrder_WithValidParameters_ReturnsInstanceWithCorrectData()
    {
        var mockOrderProductsInput = new Mock<IEnumerable<IOrderProduct>>().Object;

        var mockOrderProducts = mockOrderProductsInput.Select((input) =>
        {
            return OrderProduct.Create(
                input.ProductId,
                input.Quantity,
                5m,
                5m,
                new HashSet<CategoryId>()
                {
                    CategoryId.Create(1)
                }
            );
        }).ToHashSet();

        var mockTotal = 100m;

        OrderUtils.MockOrderService.Setup(s => s.CalculateTotalAsync(
            It.IsAny<IEnumerable<OrderProduct>>(),
            It.IsAny<IEnumerable<OrderCoupon>>())
        ).ReturnsAsync(mockTotal);

        OrderUtils.MockOrderService.Setup(s => s.PrepareOrderProductsAsync(
            It.IsAny<IEnumerable<IOrderProduct>>())
        ).Returns(mockOrderProducts.ToAsyncEnumerable());

        var act = await FluentActions
            .Invoking(() => OrderUtils.CreateOrder(
                ownerId: DomainConstants.Order.OwnerId,
                orderProducts: mockOrderProductsInput,
                billingAddress: AddressUtils.CreateAddress(),
                deliveryAddress: AddressUtils.CreateAddress(),
                installments: 1
            ))
            .Should()
            .NotThrowAsync();

        var createdOrder = act.Subject;

        createdOrder.OwnerId.Should().Be(DomainConstants.Order.OwnerId);
        createdOrder.Total.Should().Be(mockTotal);
        createdOrder.Description.Should().Be("Order pending. Waiting for payment");
        createdOrder.OrderStatusId.Should().Be(OrderStatus.Pending.Id);
        createdOrder.DomainEvents.Should().HaveCount(1);
        createdOrder.DomainEvents.Should().ContainItemsAssignableTo<OrderCreated>();
        createdOrder.OrderStatusHistories.Should().HaveCount(1);
        createdOrder.OrderStatusHistories.First().OrderStatusId.Should().Be(OrderStatus.Pending.Id);

        createdOrder.Products.Should().BeEquivalentTo(mockOrderProducts);
    }

    /// <summary>
    /// Tests that canceling an order sets the order status to canceled, updates the description to match the reason, and increments the order status history.
    /// </summary>
    [Fact]
    public async Task CancelOrder_WhenOrderIsPending_UpdateFieldsCorrectly()
    {
        var reasonToCancel = "Important reason";
        var order = await OrderUtils.CreateOrder();

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
        var order = await OrderUtils.CreateOrder();

        order.GetStatusDescription().Should().Be(OrderStatus.Pending.Name);
    }
}
