using Application.Common.PaymentGateway;
using Application.Common.Persistence.Repositories;
using Application.Orders.Errors;

using Domain.OrderAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.Orders.DTOs.Results;

namespace Application.Orders.Queries.GetOrderById;

internal sealed partial class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderDetailedResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentGateway _paymentGateway;

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

    public async Task<OrderDetailedResult> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingOrderRetrieval(request.OrderId);

        var orderId = OrderId.Create(request.OrderId);

        var orderDetailed = await _orderRepository.GetOrderDetailedAsync(
            orderId,
            cancellationToken
        );

        if (orderDetailed == null)
        {
            LogOrderNotFound();
            throw new OrderNotFoundException();
        }

        LogOrderRetrieved();

        var orderPaymentDetails = await _paymentGateway.GetPaymentByIdAsync(
            orderDetailed.PaymentId,
            cancellationToken
        );

        LogOrderPaymentDetailsRetrieved();
        LogOrderDetailedRetrievedSuccessfully();

        return OrderDetailedResult.FromProjectionWithPayment(
            orderDetailed,
            OrderPaymentResult.FromResponse(orderPaymentDetails)
        );
    }
}
