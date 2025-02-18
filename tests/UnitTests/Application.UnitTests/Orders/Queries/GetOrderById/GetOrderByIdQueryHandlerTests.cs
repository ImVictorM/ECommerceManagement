using Application.Orders.Queries.GetOrderById;
using Application.Orders.Errors;
using Application.Common.Persistence;
using Application.Common.PaymentGateway;
using Application.UnitTests.Orders.Queries.TestUtils;
using Application.UnitTests.TestUtils.PaymentGateway;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.ShippingMethodAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.PaymentAggregate;

using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Application.UnitTests.Orders.Queries.GetOrderById;

/// <summary>
/// Unit tests for the <see cref="GetOrderByIdQueryHandler"/> handler.
/// </summary>
public class GetOrderByIdQueryHandlerTests
{
    private readonly GetOrderByIdQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Payment, PaymentId>> _mockPaymentRepository;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IRepository<Shipment, ShipmentId>> _mockShipmentRepository;
    private readonly Mock<IRepository<ShippingMethod, ShippingMethodId>> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetOrderByIdQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockShippingMethodRepository = new Mock<IRepository<ShippingMethod, ShippingMethodId>>();
        _mockShipmentRepository = new Mock<IRepository<Shipment, ShipmentId>>();
        _mockPaymentRepository = new Mock<IRepository<Payment, PaymentId>>();

        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.PaymentRepository).Returns(_mockPaymentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.ShipmentRepository).Returns(_mockShipmentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.ShippingMethodRepository).Returns(_mockShippingMethodRepository.Object);

        _handler = new GetOrderByIdQueryHandler(_mockUnitOfWork.Object, _mockPaymentGateway.Object);
    }

    /// <summary>
    /// Verifies that the handler returns the order details when the order and payment exists.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WithExistingOrder_ReturnsOrderDetails()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();

        var orderId = OrderId.Create(query.OrderId);

        var order = await OrderUtils.CreateOrderAsync(id: orderId);

        var orderPayment = PaymentUtils.CreatePayment(
            paymentId: PaymentId.Create(Guid.NewGuid().ToString()),
            orderId: orderId
        );

        var paymentResponse = PaymentResponseUtils.CreateResponse();
        var shipment = ShipmentUtils.CreateShipment(id: ShipmentId.Create(1), orderId: orderId);
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(id: shipment.ShippingMethodId);

        _mockOrderRepository
            .Setup(r => r.FindByIdAsync(orderId))
            .ReturnsAsync(order);

        _mockPaymentRepository
            .Setup(r => r.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Payment, bool>>>()))
            .ReturnsAsync(orderPayment);

        _mockPaymentGateway
            .Setup(p => p.GetPaymentByIdAsync(orderPayment.Id.ToString()))
            .ReturnsAsync(paymentResponse);

        _mockShipmentRepository
            .Setup(r => r.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Shipment, bool>>>()))
            .ReturnsAsync(shipment);

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(shipment.ShippingMethodId))
            .ReturnsAsync(shippingMethod);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.Order.Should().BeEquivalentTo(order);
        result.Payment.PaymentId.Should().Be(orderPayment.Id);
        result.Payment.Amount.Should().Be(paymentResponse.Amount);
        result.Payment.Status.Should().Be(paymentResponse.Status.Name);
        result.Payment.PaymentMethod.Should().Be(paymentResponse.PaymentMethod);
        result.Payment.Details.Should().Be(paymentResponse.Details);
        result.Shipment.ShipmentId.Should().Be(shipment.Id);
        result.Shipment.Status.Should().Be(shipment.ShipmentStatus.Name);
        result.Shipment.DeliveryAddress.Should().Be(shipment.DeliveryAddress);
        result.Shipment.ShippingMethod.Name.Should().Be(shippingMethod.Name);
        result.Shipment.ShippingMethod.EstimatedDeliveryDays.Should().Be(shippingMethod.EstimatedDeliveryDays);
        result.Shipment.ShippingMethod.Price.Should().Be(shippingMethod.Price);
    }

    /// <summary>
    /// Verifies that the handler throws an exception when the order does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WhenOrderDoesNotExist_ThrowsError()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();
        var orderId = OrderId.Create(query.OrderId);

        _mockOrderRepository
            .Setup(repo => repo.FindByIdAsync(orderId))
            .ReturnsAsync((Order)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<OrderNotFoundException>();
    }
}
