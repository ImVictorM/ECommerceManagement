using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Application.Orders.DTOs;
using Application.Orders.Errors;
using Application.Orders.Queries.GetCustomerOrderById;
using Application.UnitTests.Orders.Queries.TestUtils;
using Application.UnitTests.TestUtils.PaymentGateway;
using Application.UnitTests.Orders.TestUtils;
using Application.UnitTests.Orders.TestUtils.Extensions;

using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;
using Domain.ShipmentAggregate.ValueObjects;

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
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IOrderRepository> _mockOrderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrderByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetCustomerOrderByIdQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockPaymentGateway = new Mock<IPaymentGateway>();

        var mockLogger = new Mock<ILogger<GetCustomerOrderByIdQueryHandler>>();

        _handler = new GetCustomerOrderByIdQueryHandler(
            _mockOrderRepository.Object,
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

        var userId = UserId.Create(query.UserId);

        var orderId = OrderId.Create(query.OrderId);
        var order = await OrderUtils.CreateOrderAsync(id: orderId, ownerId: userId);

        var shipment = ShipmentUtils.CreateShipment(id: ShipmentId.Create(1), orderId: orderId);
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(id: shipment.ShippingMethodId);

        var payment = PaymentUtils.CreatePayment(paymentId: PaymentId.Create("1"), orderId: orderId);
        var paymentResponse = PaymentResponseUtils.CreateResponse(paymentId: payment.Id.ToString());

        var queryResult = OrderDetailedQueryResultUtils.CreateResult(
            order,
            shipment,
            shippingMethod,
            payment.Id
        );

        _mockOrderRepository
            .Setup(repo => repo.GetOrderDetailedAsync(
                orderId,
                userId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(queryResult);

        _mockPaymentGateway
            .Setup(gateway => gateway.GetPaymentByIdAsync(payment.Id.ToString()))
            .ReturnsAsync(paymentResponse);

        var result = await _handler.Handle(query, default);

        result.EnsureCorrespondsTo(queryResult, paymentResponse);
    }

    /// <summary>
    /// Verifies an exception is thrown when the order does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerOrderById_WhenOrderDoesNotExist_ThrowsException()
    {
        var query = GetCustomerOrderByIdQueryUtils.CreateQuery();

        _mockOrderRepository
            .Setup(repo => repo.GetOrderDetailedAsync(
                It.IsAny<OrderId>(),
                It.IsAny<UserId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((OrderDetailedQueryResult)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<OrderNotFoundException>();
    }
}
