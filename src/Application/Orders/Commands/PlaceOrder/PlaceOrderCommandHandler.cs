using Application.Common.DTOs;
using Application.Common.Persistence;
using Application.Common.Security.Identity;

using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Factories;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
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
    private readonly IIdentityProvider _identityProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="orderService">The order service.</param>
    /// <param name="identityProvider">The identity provider.</param>
    public PlaceOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderService orderService,
        IIdentityProvider identityProvider
    )
    {
        _unitOfWork = unitOfWork;
        _orderService = orderService;
        _identityProvider = identityProvider;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _identityProvider.GetCurrentUserIdentity();
        var ownerId = UserId.Create(currentUser.Id);

        var orderCouponsApplied = request.CouponAppliedIds?
                .Select(CouponId.Create)
                .Select(OrderCoupon.Create);

        var orderFactory = new OrderFactory(_orderService);

        var order = await orderFactory.CreateOrderAsync(
            request.requestId,
            ownerId,
            request.Products,
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
