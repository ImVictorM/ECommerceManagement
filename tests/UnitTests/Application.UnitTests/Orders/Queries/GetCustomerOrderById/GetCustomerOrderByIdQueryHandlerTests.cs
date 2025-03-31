using Application.Common.Persistence.Repositories;
using Application.Common.PaymentGateway;
using Application.Orders.Errors;
using Application.Orders.Queries.Projections;
using Application.Orders.Queries.GetCustomerOrderById;
using Application.UnitTests.Orders.Queries.TestUtils;
using Application.UnitTests.TestUtils.PaymentGateway;
using Application.UnitTests.Orders.TestUtils.Extensions;
using Application.UnitTests.Orders.TestUtils.Projections;

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
    /// Initiates a new instance of the
    /// <see cref="GetCustomerOrderByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetCustomerOrderByIdQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockPaymentGateway = new Mock<IPaymentGateway>();

        _handler = new GetCustomerOrderByIdQueryHandler(
            _mockOrderRepository.Object,
            _mockPaymentGateway.Object,
            Mock.Of<ILogger<GetCustomerOrderByIdQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies the order containing details is retrieved when the order exists.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerOrderByIdQuery_WithExistentOrder_ReturnsOrderWithDetails()
    {
        var query = GetCustomerOrderByIdQueryUtils.CreateQuery();

        var userId = UserId.Create(query.UserId);

        var orderId = OrderId.Create(query.OrderId);
        var order = await OrderUtils.CreateOrderAsync(
            id: orderId,
            ownerId: userId
        );

        var shipment = ShipmentUtils.CreateShipment(
            id: ShipmentId.Create(1),
            orderId: orderId
        );
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(
            id: shipment.ShippingMethodId
        );

        var payment = PaymentUtils.CreatePayment(
            paymentId: PaymentId.Create("1"),
            orderId: orderId
        );

        var paymentResponse = PaymentResponseUtils.CreateResponse(
            paymentId: payment.Id
        );

        var queryProjection = OrderDetailedProjectionUtils.CreateProjection(
            order,
            shipment,
            shippingMethod,
            payment.Id
        );

        _mockOrderRepository
            .Setup(repo => repo.GetCustomerOrderDetailedAsync(
                orderId,
                userId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(queryProjection);

        _mockPaymentGateway
            .Setup(gateway => gateway.GetPaymentByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(paymentResponse);

        var result = await _handler.Handle(query, default);

        result.EnsureCorrespondsTo(queryProjection, paymentResponse);
    }

    /// <summary>
    /// Verifies an exception is thrown when the order does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerOrderByIdQuery_WhenOrderDoesNotExist_ThrowsException()
    {
        var query = GetCustomerOrderByIdQueryUtils.CreateQuery();

        _mockOrderRepository
            .Setup(repo => repo.GetCustomerOrderDetailedAsync(
                It.IsAny<OrderId>(),
                It.IsAny<UserId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((OrderDetailedProjection)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<OrderNotFoundException>();
    }
}
