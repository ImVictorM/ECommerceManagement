using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Application.Orders.Common.DTOs;
using Application.Orders.Common.Errors;

using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetCustomerOrderById;

/// <summary>
/// Handles the <see cref="GetCustomerOrderByIdQuery"/> query by
/// fetching a customer's order by id.
/// </summary>
public sealed partial class GetCustomerOrderByIdQueryHandler : IRequestHandler<GetCustomerOrderByIdQuery, OrderDetailedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrderByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="paymentGateway">The payment gateway.</param>
    /// <param name="logger">The logger.</param>
    public GetCustomerOrderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway,
        ILogger<GetCustomerOrderByIdQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<OrderDetailedResult> Handle(GetCustomerOrderByIdQuery request, CancellationToken cancellationToken)
    {
        LogHandlingOrderFetch(request.OrderId, request.UserId);

        var orderOwnerId = UserId.Create(request.UserId);
        var orderId = OrderId.Create(request.OrderId);

        var order = await _unitOfWork.OrderRepository.FindOneOrDefaultAsync(order => order.OwnerId == orderOwnerId && order.Id == orderId);

        if (order == null)
        {
            LogOrderNotFound();

            throw new OrderNotFoundException($"Order with id {orderId} and owner id {orderOwnerId} does not exist");
        }

        LogOrderFound();

        var orderPayment = await _unitOfWork.PaymentRepository.FindOneOrDefaultAsync(payment => payment.OrderId == orderId);

        PaymentResponse? paymentDetails = null;

        if (orderPayment == null)
        {
            LogOrderPaymentNotFound();
        }
        else
        {
            LogOrderPaymentFound();

            paymentDetails = await _paymentGateway.GetPaymentByIdAsync(orderPayment.Id.ToString());

            LogPaymentDetailsFetched();
        }

        LogReturningResult();

        return new OrderDetailedResult(order, paymentDetails);
    }
}
