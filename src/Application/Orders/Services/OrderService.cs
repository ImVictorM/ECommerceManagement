using Application.Common.Persistence;
using Application.Orders.Errors;

using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
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
        IEnumerable<OrderCoupon>? couponsApplied
    )
    {
        var shippingMethod = await _shippingMethodRepository.FindByIdAsync(shippingMethodId)
            ?? throw new InvalidShippingMethodException();

        var productsTotal = orderProducts.Sum(p => p.CalculateTransactionPrice());

        if (couponsApplied != null)
        {
            productsTotal = await CalculateTotalApplyingCouponsAsync(orderProducts, couponsApplied, productsTotal);
        }

        var total = productsTotal + shippingMethod.Price;

        return total;
    }

    private async Task<decimal> CalculateTotalApplyingCouponsAsync(
        IEnumerable<OrderProduct> orderProducts,
        IEnumerable<OrderCoupon> couponsApplied,
        decimal total
    )
    {
        var couponAppliedIds = couponsApplied.Select(c => c.CouponId).ToList();

        var coupons = await _couponRepository.FindAllAsync(c => couponAppliedIds.Contains(c.Id));

        if (coupons.Count() != couponAppliedIds.Count)
        {
            throw new InvalidCouponAppliedException("Some of the applied coupons are expired");
        }

        var products = orderProducts.Select(p => (p.ProductId, p.ProductCategoryIds)).ToHashSet();

        var couponsCanBeApplied = coupons.All(c => c.CanBeApplied(CouponOrder.Create(products, total)));

        if (!couponsCanBeApplied)
        {
            throw new InvalidCouponAppliedException("Some of the applied coupons are expired or invalid");
        }

        var couponDiscounts = coupons.Select(c => c.Discount);

        return DiscountService.ApplyDiscounts(total, [.. couponDiscounts]);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<OrderProduct> PrepareOrderProductsAsync(IEnumerable<IOrderProductReserved> orderProducts)
    {
        var products = await FetchProductsAsync(orderProducts.Select(p => p.ProductId));

        foreach (var op in orderProducts)
        {
            var product = products[op.ProductId];

            product.Inventory.RemoveStock(op.Quantity);

            var productPrice = await _productService.CalculateProductPriceApplyingSaleAsync(product);

            yield return OrderProduct.Create(
                op.ProductId,
                op.Quantity,
                product.BasePrice,
                productPrice,
                product.ProductCategories.Select(p => p.CategoryId).ToHashSet()
            );
        }
    }

    private async Task<IDictionary<ProductId, Product>> FetchProductsAsync(IEnumerable<ProductId> productIds)
    {
        var products = await _productRepository.FindAllAsync(p => productIds.Contains(p.Id));

        if (products.Count() != productIds.Count())
        {
            throw new OrderProductNotAvailableException("Some of the order products could not be found");
        }

        return products.ToDictionary(p => p.Id);
    }
}
