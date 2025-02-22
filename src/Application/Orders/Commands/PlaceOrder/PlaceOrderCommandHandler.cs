using Application.Common.DTOs;
using Application.Common.Persistence;
using Application.Common.Security.Identity;

using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Factories;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Orders.Commands.PlaceOrder;

/// <summary>
/// Handles the process of placing an order.
/// </summary>
public sealed partial class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IIdentityProvider _identityProvider;
    private readonly OrderFactory _orderFactory;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="orderService">The order service.</param>
    /// <param name="identityProvider">The identity provider.</param>
    /// <param name="logger">The logger.</param>
    public PlaceOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IOrderService orderService,
        IIdentityProvider identityProvider,
        ILogger<PlaceOrderCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _identityProvider = identityProvider;
        _orderFactory = new OrderFactory(orderService);
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingPlaceOrder();

        var currentUser = _identityProvider.GetCurrentUserIdentity();
        var ownerId = UserId.Create(currentUser.Id);

        LogOrderCustomerId(currentUser.Id);

        var shippingMethodId = ShippingMethodId.Create(request.ShippingMethodId);

        var orderCouponsApplied = request.CouponAppliedIds?
                .Select(CouponId.Create)
                .Select(OrderCoupon.Create);

        var order = await _orderFactory.CreateOrderAsync(
            request.RequestId,
            ownerId,
            shippingMethodId,
            request.Products,
            request.PaymentMethod,
            request.BillingAddress,
            request.DeliveryAddress,
            request.Installments,
            orderCouponsApplied
        );

        LogOrderCreated();

        await _orderRepository.AddAsync(order);

        await _unitOfWork.SaveChangesAsync();

        LogOrderPlacedSuccessfully();

        return new CreatedResult(order.Id.ToString());
    }
}
