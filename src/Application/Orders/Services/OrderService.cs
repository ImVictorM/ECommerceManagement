using Application.Common.Persistence;
using Application.Orders.Errors;

using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.Services;
using Domain.ShippingMethodAggregate.ValueObjects;

using SharedKernel.Services;

namespace Application.Orders.Services;

/// <summary>
/// Represents services related to order products.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IProductService _productService;
    private readonly IShippingMethodRepository _shippingMethodRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="productService">The product service.</param>
    /// <param name="couponRepository">The coupon repository.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="shippingMethodRepository">The shipping method repository.</param>
    public OrderService(
        IProductService productService,
        IShippingMethodRepository shippingMethodRepository,
        ICouponRepository couponRepository,
        IProductRepository productRepository

    )
    {
        _productService = productService;
        _shippingMethodRepository = shippingMethodRepository;
        _couponRepository = couponRepository;
        _productRepository = productRepository;
    }

    /// <inheritdoc/>
    public async Task<decimal> CalculateTotalAsync(
        IEnumerable<OrderProduct> orderProducts,
        ShippingMethodId shippingMethodId,
        IEnumerable<OrderCoupon>? couponsApplied,
        CancellationToken cancellationToken = default
    )
    {
        var shippingMethod = await _shippingMethodRepository.FindByIdAsync(shippingMethodId, cancellationToken)
            ?? throw new InvalidShippingMethodException();

        var productsTotal = orderProducts.Sum(p => p.CalculateTransactionPrice());

        if (couponsApplied != null)
        {
            productsTotal = await CalculateTotalApplyingCouponsAsync(
                orderProducts,
                couponsApplied,
                productsTotal,
                cancellationToken
            );
        }

        var total = productsTotal + shippingMethod.Price;

        return total;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OrderProduct>> PrepareOrderProductsAsync(
        IEnumerable<IOrderProductReserved> orderProducts,
        CancellationToken cancellationToken = default
    )
    {
        var productIds = orderProducts.Select(p => p.ProductId);

        var products = await _productRepository.FindAllAsync(
            p => productIds.Contains(p.Id),
            cancellationToken
        );

        var productOnSalePrices = await _productService.CalculateProductsPriceApplyingSaleAsync(
            products,
            cancellationToken
        );

        var productsMap = products.ToDictionary(p => p.Id);

        foreach (var op in orderProducts)
        {
            try
            {
                var product = productsMap[op.ProductId];

                product.Inventory.RemoveStock(op.Quantity);
            }
            catch (Exception)
            {
                throw new OrderProductNotAvailableException();
            }
        }

        return orderProducts.Select(op => OrderProduct.Create(
            op.ProductId,
            op.Quantity,
            productsMap[op.ProductId].BasePrice,
            productOnSalePrices[op.ProductId],
            productsMap[op.ProductId].ProductCategories.Select(c => c.CategoryId).ToHashSet()
        ));
    }

    private async Task<decimal> CalculateTotalApplyingCouponsAsync(
        IEnumerable<OrderProduct> orderProducts,
        IEnumerable<OrderCoupon> couponsApplied,
        decimal total,
        CancellationToken cancellationToken = default
    )
    {
        var couponIds = couponsApplied.Select(c => c.CouponId);

        var coupons = await _couponRepository.FindAllAsync(
            c => couponIds.Contains(c.Id),
            cancellationToken
        );

        var couponsMap = coupons.ToDictionary(c => c.Id);

        var productsWithCategoryIds = orderProducts.Select(p => (p.ProductId, p.ProductCategoryIds)).ToHashSet();

        foreach (var couponId in couponIds)
        {
            try
            {
                var coupon = couponsMap[couponId];
                var couponCanBeApplied = coupon.CanBeApplied(CouponOrder.Create(productsWithCategoryIds, total));

                if (!couponCanBeApplied)
                {
                    throw new InvalidCouponAppliedException(
                        $"The coupon with id {coupon.Id} cannot be applied" +
                        $" because the order does not meet the requirements"
                    );
                }
            }
            catch (Exception)
            {
                throw new InvalidCouponAppliedException(
                    $"The coupon with id {couponId}" +
                    $" Is expired or invalid"
                );
            }
        }

        var couponDiscounts = coupons.Select(c => c.Discount);

        return DiscountService.ApplyDiscounts(total, [.. couponDiscounts]);
    }
}
