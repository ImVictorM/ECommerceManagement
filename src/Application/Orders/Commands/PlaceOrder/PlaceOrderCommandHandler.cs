using Application.Common.DTOs;
using Application.Common.Interfaces.Persistence;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Factories;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Application.Orders.Commands.PlaceOrder;

/// <summary>
/// Handles the process of placing an order.
/// </summary>
public sealed class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderService _orderService;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="orderService">The order service.</param>
    public PlaceOrderCommandHandler(IUnitOfWork unitOfWork, IOrderService orderService)
    {
        _unitOfWork = unitOfWork;
        _orderService = orderService;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var ownerId = UserId.Create(request.CurrentUserId);

        var orderProducts = request.Products
            .Select(p => OrderProduct.Create(ProductId.Create(p.Id), p.Quantity))
            .ToList();

        var orderCouponsApplied = request.CouponAppliedIds?
                .Select(CouponId.Create)
                .Select(OrderCoupon.Crete);

        var orderFactory = new OrderFactory(_orderService);

        var order = await orderFactory.CreateOrderAsync(
            ownerId,
            orderProducts,
            request.PaymentMethod,
            request.BillingAddress,
            request.DeliveryAddress,
            request.Installments,
            orderCouponsApplied
        );

        await _unitOfWork.OrderRepository.AddAsync(order);

        await _unitOfWork.SaveChangesAsync();

        return new CreatedResult(order.Id.ToString());
    }
}
