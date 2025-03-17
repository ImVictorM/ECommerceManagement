using Application.Common.Persistence.Repositories;
using Application.Orders.Errors;

using Domain.CouponAggregate.Services;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;

namespace Application.Orders.Services;

internal sealed class OrderPricingService : IOrderPricingService
{
    private readonly IShippingMethodRepository _shippingMethodRepository;
    private readonly ICouponApplicationService _couponApplicationService;

    public OrderPricingService(
        IShippingMethodRepository shippingMethodRepository,
        ICouponApplicationService couponApplicationService
    )
    {
        _shippingMethodRepository = shippingMethodRepository;
        _couponApplicationService = couponApplicationService;
    }

    /// <inheritdoc/>
    public async Task<decimal> CalculateTotalAsync(
        IEnumerable<OrderLineItem> lineItems,
        ShippingMethodId shippingMethodId,
        IEnumerable<OrderCoupon>? couponsApplied = null,
        CancellationToken cancellationToken = default
    )
    {
        var shippingMethod = await _shippingMethodRepository
            .FindByIdAsync(shippingMethodId, cancellationToken)
            ?? throw new InvalidShippingMethodException();

        var productsTotal = lineItems.Sum(p => p.CalculateTransactionPrice());

        if (couponsApplied != null && couponsApplied.Any())
        {
            productsTotal = await CalculateTotalApplyingCouponsAsync(
                lineItems,
                couponsApplied,
                productsTotal,
                cancellationToken
            );
        }

        var total = productsTotal + shippingMethod.Price;

        return total;
    }

    private async Task<decimal> CalculateTotalApplyingCouponsAsync(
        IEnumerable<OrderLineItem> lineItems,
        IEnumerable<OrderCoupon> couponsApplied,
        decimal total,
        CancellationToken cancellationToken = default
    )
    {
        var couponIds = couponsApplied.Select(c => c.CouponId);

        var couponOrderProducts = lineItems
            .Select(p => CouponOrderProduct.Create(
                p.ProductId,
                p.ProductCategoryIds
            ))
            .ToHashSet();

        var couponOrder = CouponOrder.Create(
            couponOrderProducts,
            total
        );

        return await _couponApplicationService.ApplyCouponsAsync(
            couponOrder,
            couponIds,
            cancellationToken
        );
    }
}
