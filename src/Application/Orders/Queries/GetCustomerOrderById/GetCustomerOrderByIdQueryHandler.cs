using Application.Common.PaymentGateway;
using Application.Common.Persistence.Repositories;
using Application.Orders.DTOs;
using Application.Orders.Errors;

using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Orders.Queries.GetCustomerOrderById;

internal sealed partial class GetCustomerOrderByIdQueryHandler
    : IRequestHandler<GetCustomerOrderByIdQuery, OrderDetailedResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentGateway _paymentGateway;

    public GetCustomerOrderByIdQueryHandler(
        IOrderRepository orderRepository,
        IPaymentGateway paymentGateway,
        ILogger<GetCustomerOrderByIdQueryHandler> logger
    )
    {
        _orderRepository = orderRepository;
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<OrderDetailedResult> Handle(GetCustomerOrderByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingCustomerOrderRetrieval(request.OrderId, request.UserId);

        var orderOwnerId = UserId.Create(request.UserId);
        var orderId = OrderId.Create(request.OrderId);

        var orderDetailed = await _orderRepository.GetOrderDetailedAsync(orderId, orderOwnerId, cancellationToken);

        if (orderDetailed == null)
        {
            LogOrderNotFound();

            throw new OrderNotFoundException($"Order with id {orderId} and owner id {orderOwnerId} does not exist");
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
