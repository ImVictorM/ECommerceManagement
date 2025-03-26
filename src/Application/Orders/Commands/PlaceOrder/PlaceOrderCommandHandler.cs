using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Common.Persistence.Repositories;
using Application.Common.DTOs.Results;

using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Factories;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Orders.Commands.PlaceOrder;

internal sealed partial class PlaceOrderCommandHandler
    : IRequestHandler<PlaceOrderCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IIdentityProvider _identityProvider;
    private readonly OrderFactory _orderFactory;

    public PlaceOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IOrderAssemblyService orderAssemblyService,
        IOrderPricingService orderPricingService,
        IIdentityProvider identityProvider,
        ILogger<PlaceOrderCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _identityProvider = identityProvider;
        _orderFactory = new OrderFactory(
            orderAssemblyService,
            orderPricingService
        );
        _logger = logger;
    }

    public async Task<CreatedResult> Handle(
        PlaceOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingOrderPlacement();

        var currentUser = _identityProvider.GetCurrentUserIdentity();

        var ownerId = UserId.Create(currentUser.Id);

        LogOrderOwnerIdentifier(currentUser.Id);

        var shippingMethodId = ShippingMethodId.Create(request.ShippingMethodId);

        var orderCouponsApplied = request.CouponAppliedIds?
                .Select(CouponId.Create)
                .Select(OrderCoupon.Create);

        var orderLineItemDrafts = request.Products
            .Select(input => OrderLineItemDraft.Create(
                ProductId.Create(input.ProductId),
                input.Quantity
            ));

        var order = await _orderFactory.CreateOrderAsync(
            request.RequestId,
            ownerId,
            shippingMethodId,
            orderLineItemDrafts,
            request.PaymentMethod,
            request.BillingAddress,
            request.DeliveryAddress,
            request.Installments,
            orderCouponsApplied,
            cancellationToken
        );

        LogOrderCreated();

        await _orderRepository.AddAsync(order);

        await _unitOfWork.SaveChangesAsync();

        LogOrderPlacedSuccessfully();

        return new CreatedResult(order.Id.ToString());
    }
}
