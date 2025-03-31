using Application.Orders.Queries.GetOrderById;
using Application.Orders.Errors;
using Application.Orders.Queries.Projections;
using Application.Common.Persistence.Repositories;
using Application.Common.PaymentGateway;
using Application.UnitTests.Orders.Queries.TestUtils;
using Application.UnitTests.TestUtils.PaymentGateway;
using Application.UnitTests.Orders.TestUtils.Extensions;

using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using Application.UnitTests.Orders.TestUtils.Projections;

namespace Application.UnitTests.Orders.Queries.GetOrderById;

/// <summary>
/// Unit tests for the <see cref="GetOrderByIdQueryHandler"/> handler.
/// </summary>
public class GetOrderByIdQueryHandlerTests
{
    private readonly GetOrderByIdQueryHandler _handler;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="GetOrderByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetOrderByIdQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockPaymentGateway = new Mock<IPaymentGateway>();

        _handler = new GetOrderByIdQueryHandler(
            _mockOrderRepository.Object,
            _mockPaymentGateway.Object,
            Mock.Of<ILogger<GetOrderByIdQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies the handler returns the order with details when the order exists.
    /// </summary>
    [Fact]
    public async Task HandleGetOrderByIdQuery_WithExistentOrder_ReturnsOrderDetails()
    {
        var query = GetOrderByIdQueryUtils.CreateQuery();

        var orderId = OrderId.Create(query.OrderId);
        var order = await OrderUtils.CreateOrderAsync(id: orderId);

        var shipment = ShipmentUtils.CreateShipment(
            id: ShipmentId.Create(1),
            orderId: orderId
        );

        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(
            id: shipment.ShippingMethodId
        );

        var payment = PaymentUtils.CreatePayment(
            paymentId: PaymentId.Create(Guid.NewGuid().ToString()),
            orderId: orderId
        );
        var paymentDetailedResponse = PaymentResponseUtils
            .CreateResponse(paymentId: payment.Id);

        var orderDetailedProjection = OrderDetailedProjectionUtils.CreateProjection(
            order,
            shipment,
            shippingMethod,
            payment.Id
        );

        _mockOrderRepository
            .Setup(r => r.GetOrderDetailedAsync(
                orderId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(orderDetailedProjection);

        _mockPaymentGateway
            .Setup(p => p.GetPaymentByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(paymentDetailedResponse);

        var result = await _handler.Handle(query, default);

        result.EnsureCorrespondsTo(orderDetailedProjection, paymentDetailedResponse);
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
            .Setup(repo => repo.GetOrderDetailedAsync(
                orderId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((OrderDetailedProjection?)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<OrderNotFoundException>();
    }
}
