using Domain.CategoryAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;
using Moq;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.UnitTests.OrderAggregate;

/// <summary>
/// Tests for the <see cref="Order"/> aggregate root.
/// </summary>
public class OrderTests
{
    /// <summary>
    /// Represents valid parameters to create an order.
    /// </summary>
    public static IEnumerable<object[]> ValidOrderParameters()
    {
        yield return new object[]
        {
            DomainConstants.Order.OwnerId,
            new List<OrderUtils.OrderProductInput>()
            {
                new()
                {
                    ProductId = ProductId.Create(1),
                    Quantity = 5
                },
            },
            PaymentUtils.CreateCreditCardPayment(),
            AddressUtils.CreateAddress(),
            AddressUtils.CreateAddress(),
            DomainConstants.Payment.Installments
        };
    }

    /// <summary>
    /// Tests the order is created correctly when creating it with correct parameters.
    /// </summary>
    /// <param name="ownerId">The order owner id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The order payment installments.</param>
    [Theory]
    [MemberData(nameof(ValidOrderParameters))]
    public async Task Order_WhenCreatingWithValidParameters_ReturnsInstanceWithCorrectData(
        UserId ownerId,
        IEnumerable<IOrderProduct> orderProducts,
        IPaymentMethod paymentMethod,
        Address billingAddress,
        Address deliveryAddress,
        int installments
    )
    {
        var mockTotal = 100m;
        var mockOrderProducts = orderProducts.Select((input) =>
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
        });

        OrderUtils.MockOrderService.Setup(s => s.CalculateTotalAsync(
            It.IsAny<IEnumerable<OrderProduct>>(),
            It.IsAny<IEnumerable<OrderCoupon>>())
        ).ReturnsAsync(mockTotal);

        OrderUtils.MockOrderService.Setup(s => s.PrepareOrderProductsAsync(
            It.IsAny<IEnumerable<IOrderProduct>>())
        ).ReturnsAsync(mockOrderProducts);

        var act = await FluentActions
            .Invoking(() => OrderUtils.CreateOrder(
                ownerId: ownerId,
                orderProducts: orderProducts,
                paymentMethod: paymentMethod,
                billingAddress: billingAddress,
                deliveryAddress: deliveryAddress,
                installments: installments
            ))
            .Should()
            .NotThrowAsync();

        var createdOrder = act.Subject;

        createdOrder.OwnerId.Should().Be(ownerId);
        createdOrder.Total.Should().Be(mockTotal);
        createdOrder.Description.Should().Be("Order pending. Waiting for payment");
        createdOrder.OrderStatusId.Should().Be(OrderStatus.Pending.Id);
        createdOrder.DomainEvents.Should().HaveCount(1);
        createdOrder.DomainEvents.Should().ContainItemsAssignableTo<OrderCreated>();
        createdOrder.OrderStatusHistories.Should().HaveCount(1);
        createdOrder.OrderStatusHistories.First().OrderStatusId.Should().Be(OrderStatus.Pending.Id);

        createdOrder.Products.Count.Should().Be(orderProducts.Count());
        createdOrder.Products.Should().BeEquivalentTo(mockOrderProducts);
    }

    /// <summary>
    /// Tests that canceling an order sets the order status to canceled, updates the description to match the reason, and increments the order status history.
    /// </summary>
    [Fact]
    public async Task Order_WhenCancelingAnOrder_SetStatusAndChangeDescription()
    {
        var reasonToCancel = "Important reason";
        var order = await OrderUtils.CreateOrder();

        order.CancelOrder(reasonToCancel);

        order.OrderStatusId.Should().Be(OrderStatus.Canceled.Id);
        order.Description.Should().Be(reasonToCancel);
        order.OrderStatusHistories.Count.Should().Be(2);
        order.OrderStatusHistories.Should().Contain(osh => osh.OrderStatusId == OrderStatus.Canceled.Id);
    }
}
