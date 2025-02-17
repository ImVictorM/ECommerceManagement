using Application.Orders.Queries.GetOrderById;
using Application.Orders.Errors;
using Application.Common.Persistence;
using Application.Common.PaymentGateway;
using Application.UnitTests.Orders.Queries.TestUtils;
using Application.UnitTests.TestUtils.PaymentGateway;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
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

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetOrderByIdQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockPaymentRepository = new Mock<IRepository<Payment, PaymentId>>();

        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.PaymentRepository).Returns(_mockPaymentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);

        _handler = new GetOrderByIdQueryHandler(_mockUnitOfWork.Object, _mockPaymentGateway.Object);
    }

    /// <summary>
    /// Verifies that the handler returns the order details when the order and payment exists.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WhenOrderAndPaymentExists_OrderWithPayment()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();

        var orderId = OrderId.Create(query.OrderId);

        var order = await OrderUtils.CreateOrderAsync(id: orderId);

        var orderPayment = PaymentUtils.CreatePayment(
            paymentId: PaymentId.Create(Guid.NewGuid().ToString()),
            orderId: orderId
        );

        var paymentResponse = PaymentResponseUtils.CreateResponse();

        _mockOrderRepository
            .Setup(r => r.FindByIdAsync(orderId))
            .ReturnsAsync(order);

        _mockPaymentRepository
            .Setup(r => r.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Payment, bool>>>()))
            .ReturnsAsync(orderPayment);

        _mockPaymentGateway
            .Setup(p => p.GetPaymentByIdAsync(orderPayment.Id.ToString()))
            .ReturnsAsync(paymentResponse);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.Order.Should().BeEquivalentTo(order);
        result.Payment.Should().BeEquivalentTo(paymentResponse);
    }

    /// <summary>
    /// Verifies that the handler returns order details without payment when no payment is found.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WhenOrderExistsButPaymentNot_ReturnsOrderWithoutPayment()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();
        var orderId = OrderId.Create(query.OrderId);

        var order = await OrderUtils.CreateOrderAsync(id: orderId);

        _mockOrderRepository
            .Setup(repo => repo.FindByIdAsync(orderId))
            .ReturnsAsync(order);

        _mockPaymentRepository
            .Setup(r => r.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Payment, bool>>>()))
            .ReturnsAsync((Payment)null!);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.Order.Should().BeEquivalentTo(order);
        result.Payment.Should().BeNull();
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
