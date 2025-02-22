using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Application.Orders.DTOs;
using Application.Orders.Errors;

using Domain.OrderAggregate.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetOrderById;

/// <summary>
/// Query handler for the <see cref="GetOrderByIdQuery"/> query.
/// </summary>
public sealed partial class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailedResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentGateway _paymentGateway;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="paymentGateway">The payment gateway.</param>
    /// <param name="logger">The logger.</param>
    public GetOrderByIdQueryHandler(
        IOrderRepository orderRepository,
        IPaymentGateway paymentGateway,
        ILogger<GetOrderByIdQueryHandler> logger
    )
    {
        _orderRepository = orderRepository;
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<OrderDetailedResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingOrderRetrieval(request.OrderId);

        var orderId = OrderId.Create(request.OrderId);

        var orderDetailed = await _orderRepository.GetOrderDetailedAsync(orderId, cancellationToken);

        if (orderDetailed == null)
        {
            LogOrderNotFound();
            throw new OrderNotFoundException();
        }

        LogOrderRetrieved();

        var orderPaymentDetails = await _paymentGateway.GetPaymentByIdAsync(orderDetailed.PaymentId.ToString());

        LogOrderPaymentDetailsRetrieved();

        LogOrderDetailedRetrievedSuccessfully();

        return new OrderDetailedResult(
            orderDetailed.Order,
            orderDetailed.OrderShipment,
            new OrderPaymentResult(
                orderDetailed.PaymentId,
                orderPaymentDetails.Amount,
                orderPaymentDetails.Installments,
                orderPaymentDetails.Status.Name,
                orderPaymentDetails.Details,
                orderPaymentDetails.PaymentMethod
            )
        );
    }
}
