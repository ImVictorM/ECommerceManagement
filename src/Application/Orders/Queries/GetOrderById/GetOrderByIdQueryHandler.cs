using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Application.Orders.DTOs;
using Application.Orders.Errors;

using Domain.OrderAggregate.ValueObjects;

using MediatR;

namespace Application.Orders.Queries.GetOrderById;

/// <summary>
/// Query handler for the <see cref="GetOrderByIdQuery"/> query.
/// </summary>
public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="paymentGateway">The payment gateway.</param>
    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IPaymentGateway paymentGateway)
    {
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
    }

    /// <inheritdoc/>
    public async Task<OrderDetailedResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Create(request.OrderId);

        var order =
            await _unitOfWork.OrderRepository.FindByIdAsync(orderId)
            ?? throw new OrderNotFoundException();

        var orderPayment = await _unitOfWork.PaymentRepository.FindOneOrDefaultAsync(payment => payment.OrderId == order.Id);

        if (orderPayment == null)
        {
            return new OrderDetailedResult(order, null);
        }

        var orderPaymentDetails = await _paymentGateway.GetPaymentByIdAsync(orderPayment.Id.ToString());

        return new OrderDetailedResult(order, orderPaymentDetails);
    }
}
