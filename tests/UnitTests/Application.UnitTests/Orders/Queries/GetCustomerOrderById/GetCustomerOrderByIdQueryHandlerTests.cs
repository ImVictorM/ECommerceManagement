using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Application.Orders.Queries.GetCustomerOrderById;
using Application.UnitTests.Orders.Queries.TestUtils;
using Application.UnitTests.TestUtils.PaymentGateway;
using Application.Orders.Errors;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Orders.Queries.GetCustomerOrderById;

/// <summary>
/// Unit tests for the <see cref="GetCustomerOrderByIdQueryHandler"/> handler.
/// </summary>
public class GetCustomerOrderByIdQueryHandlerTests
{
    private readonly GetCustomerOrderByIdQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;
    private readonly Mock<IRepository<Payment, PaymentId>> _mockPaymentRepository;
    private readonly Mock<IRepository<Shipment, ShipmentId>> _mockShipmentRepository;
    private readonly Mock<IRepository<ShippingMethod, ShippingMethodId>> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrderByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetCustomerOrderByIdQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockPaymentRepository = new Mock<IRepository<Payment, PaymentId>>();
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockShippingMethodRepository = new Mock<IRepository<ShippingMethod, ShippingMethodId>>();
        _mockShipmentRepository = new Mock<IRepository<Shipment, ShipmentId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        var mockLogger = new Mock<ILogger<GetCustomerOrderByIdQueryHandler>>();

        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.PaymentRepository).Returns(_mockPaymentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.ShipmentRepository).Returns(_mockShipmentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.ShippingMethodRepository).Returns(_mockShippingMethodRepository.Object);

        _handler = new GetCustomerOrderByIdQueryHandler(
            _mockUnitOfWork.Object,
            _mockPaymentGateway.Object,
            mockLogger.Object
        );
    }

    /// <summary>
    /// Verifies the order and the order payment is retrieved when they both exist.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerOrderById_WithExistingOrder_ReturnsOrderWithDetails()
    {
        var query = GetCustomerOrderByIdQueryUtils.CreateQuery();

        var orderId = OrderId.Create(query.OrderId);
        var userId = UserId.Create(query.UserId);

        var order = await OrderUtils.CreateOrderAsync(id: orderId, ownerId: userId);
        var payment = PaymentUtils.CreatePayment(paymentId: PaymentId.Create("1"), orderId: orderId);
        var paymentResponse = PaymentResponseUtils.CreateResponse(paymentId: payment.Id.ToString());
        var shipment = ShipmentUtils.CreateShipment(id: ShipmentId.Create(1), orderId: orderId);
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(id: shipment.ShippingMethodId);

        _mockOrderRepository
            .Setup(repo => repo.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(order);

        _mockPaymentRepository
            .Setup(repo => repo.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Payment, bool>>>()))
            .ReturnsAsync(payment);

        _mockPaymentGateway
            .Setup(gateway => gateway.GetPaymentByIdAsync(payment.Id.ToString()))
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
        result.Payment.PaymentId.Should().Be(payment.Id);
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
    /// Verifies an exception is thrown when the order does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerOrderById_WhenOrderDoesNotExist_ThrowsException()
    {
        var query = GetCustomerOrderByIdQueryUtils.CreateQuery();

        _mockOrderRepository
            .Setup(repo => repo.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync((Order)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<OrderNotFoundException>();
    }
}
