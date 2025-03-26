using Application.Common.PaymentGateway;
using Application.Common.Persistence.Repositories;
using Application.Orders.Errors;

using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.Orders.DTOs.Results;

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

    public async Task<OrderDetailedResult> Handle(
        GetCustomerOrderByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCustomerOrderRetrieval(request.OrderId, request.OwnerId);

        var ownerId = UserId.Create(request.OwnerId);
        var orderId = OrderId.Create(request.OrderId);

        var orderWithDetails = await _orderRepository.GetCustomerOrderDetailedAsync(
            orderId,
            ownerId,
            cancellationToken
        );

        if (orderWithDetails == null)
        {
            LogOrderNotFound();

            throw new OrderNotFoundException(
                "The customer order could not be found"
            )
            .WithContext("OrderId", request.OrderId)
            .WithContext("OwnerId", request.OwnerId);
        }

        LogOrderRetrieved();

        var orderPaymentDetails = await _paymentGateway.GetPaymentByIdAsync(
            orderWithDetails.PaymentId,
            cancellationToken
        );

        LogOrderPaymentDetailsRetrieved();
        LogOrderDetailedRetrievedSuccessfully();

        return OrderDetailedResult.FromProjectionWithPayment(
            orderWithDetails,
            OrderPaymentResult.FromResponse(orderPaymentDetails)
        );
    }
}
