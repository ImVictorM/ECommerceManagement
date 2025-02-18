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

        var orderPaymentDetails = await _paymentGateway.GetPaymentByIdAsync(orderPayment!.Id.ToString());

        var orderShipment = await _unitOfWork.ShipmentRepository.FindOneOrDefaultAsync(shipment => shipment.OrderId == order.Id);

        var orderShippingMethod = await _unitOfWork.ShippingMethodRepository.FindByIdAsync(orderShipment!.ShippingMethodId);

        return new OrderDetailedResult(
            order,
            new OrderPaymentResult(
                orderPayment.Id,
                orderPaymentDetails.Amount,
                orderPaymentDetails.Installments,
                orderPaymentDetails.Status.Name,
                orderPaymentDetails.Details,
                orderPaymentDetails.PaymentMethod
            ),
            new OrderShipmentResult(
                orderShipment.Id,
                orderShipment.ShipmentStatus.Name,
                orderShipment.DeliveryAddress,
                new OrderShippingMethodResult(
                    orderShippingMethod!.Name,
                    orderShippingMethod.Price,
                    orderShippingMethod.EstimatedDeliveryDays
                )
            )
        );
    }
}
