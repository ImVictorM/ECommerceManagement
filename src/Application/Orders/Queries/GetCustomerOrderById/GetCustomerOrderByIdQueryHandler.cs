using Application.Common.PaymentGateway;
using Application.Common.Persistence.Repositories;
using Application.Orders.Errors;
using Application.Orders.DTOs.Results;

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

    public async Task<OrderDetailedResult> Handle(
        GetCustomerOrderByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCustomerOrderRetrieval(request.OrderId, request.UserId);

        var ownerId = UserId.Create(request.UserId);
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
            .WithContext("OwnerId", request.UserId);
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
