using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

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
            DomainConstants.Order.OrderProducts,
            PaymentUtils.CreateCreditCardPayment(),
            DomainConstants.Order.Total,
            AddressUtils.CreateAddress(),
            AddressUtils.CreateAddress(),
            DomainConstants.Payment.Installments
        };
    }

    /// <summary>
    /// Tests the order is created correctly when creating it with correct parameters.
    /// </summary>
    /// <param name="userId">The order owner id.</param>
    /// <param name="orderProducts">The order products.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="total">The order total.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The order payment installments.</param>
    [Theory]
    [MemberData(nameof(ValidOrderParameters))]
    public void Order_WhenCreatingWithValidParameters_ReturnsInstanceWithCorrectData(
        UserId userId,
        IEnumerable<OrderProduct> orderProducts,
        IPaymentMethod paymentMethod,
        decimal total,
        Address billingAddress,
        Address deliveryAddress,
        int installments
    )
    {
        var act = FluentActions
            .Invoking(() => Order.Create(
                userId,
                orderProducts,
                total,
                paymentMethod,
                billingAddress,
                deliveryAddress,
                installments
            ))
            .Should()
            .NotThrow();

        var createdOrder = act.Subject;

        createdOrder.OwnerId.Should().Be(userId);
        createdOrder.Total.Should().Be(total);
        createdOrder.Description.Should().Be("Order pending. Waiting for payment");
        createdOrder.OrderStatusId.Should().Be(OrderStatus.Pending.Id);
        createdOrder.DomainEvents.Should().HaveCount(1);
        createdOrder.DomainEvents.Should().ContainItemsAssignableTo<OrderCreated>();
        createdOrder.OrderStatusHistories.Should().HaveCount(1);
        createdOrder.OrderStatusHistories[0].OrderStatusId.Should().Be(OrderStatus.Pending.Id);
        createdOrder.Products.Should().BeEquivalentTo(orderProducts);
    }

    /// <summary>
    /// Tests that canceling an order sets the order status to canceled, updates the description to match the reason, and increments the order status history.
    /// </summary>
    [Fact]
    public void Order_WhenCancelingAnOrder_SetStatusAndChangeDescription()
    {
        var reasonToCancel = "Important reason";
        var order = OrderUtils.CreateOrder();

        order.CancelOrder(reasonToCancel);

        order.OrderStatusId.Should().Be(OrderStatus.Canceled.Id);
        order.Description.Should().Be(reasonToCancel);
        order.OrderStatusHistories.Count.Should().Be(2);
        order.OrderStatusHistories.Should().Contain(osh => osh.OrderStatusId == OrderStatus.Canceled.Id);
    }
}
