using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Orders.Common.Errors;

using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Services;

namespace Application.Orders.Services;

/// <summary>
/// Represents services related to order products.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productService">The product service.</param>
    public OrderService(
        IUnitOfWork unitOfWork,
        IProductService productService
    )
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
    }

    /// <inheritdoc/>
    public async Task<decimal> CalculateTotalAsync(
        IEnumerable<OrderProduct> orderProducts,
        IEnumerable<OrderCoupon>? couponsApplied
    )
    {
        var total = orderProducts.Sum(p => p.CalculateTransactionPrice());

        if (couponsApplied == null)
        {
            return total;
        }

        var totalAfterCouponDiscounts = await CalculateTotalApplyingCouponsAsync(orderProducts, couponsApplied, total);

        return totalAfterCouponDiscounts;
    }

    private async Task<decimal> CalculateTotalApplyingCouponsAsync(
        IEnumerable<OrderProduct> orderProducts,
        IEnumerable<OrderCoupon> couponsApplied,
        decimal total
    )
    {
        var couponAppliedIds = couponsApplied.Select(c => c.CouponId);

        var coupons = await _unitOfWork.CouponRepository.FindAllAsync(c => couponAppliedIds.Contains(c.Id));

        if (coupons.Count() != couponsApplied.Count())
        {
            throw new InvalidOrderCouponAppliedException("Some of the applied coupons are expired");
        }

        var products = orderProducts.Select(p => (p.ProductId, p.ProductCategoryIds)).ToHashSet();

        var couponsCanBeApplied = coupons.All(c => c.CanBeApplied(CouponOrder.Create(products, total)));

        if (!couponsCanBeApplied)
        {
            throw new InvalidOrderCouponAppliedException("Some of the applied coupons are expired or invalid");
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
        var products = await _unitOfWork.ProductRepository.FindAllAsync(p => productIds.Contains(p.Id));

        if (products.Count() != productIds.Count())
        {
            throw new ProductNotFoundException("Some of the order products could not be found");
        }

        return products.ToDictionary(p => p.Id);
    }
}
