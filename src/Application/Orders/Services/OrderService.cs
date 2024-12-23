using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Orders.Common.Errors;

using Domain.CouponAggregate.Services;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;

using SharedKernel.Services;
using SharedKernel.ValueObjects;

namespace Application.Orders.Services;

/// <summary>
/// Represents services related to order products.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;
    private readonly ICouponService _couponService;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productService">The product service.</param>
    /// <param name="couponService">The coupon service.</param>
    public OrderService(
        IUnitOfWork unitOfWork,
        IProductService productService,
        ICouponService couponService
    )
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
        _couponService = couponService;
    }

    /// <inheritdoc/>
    public async Task<bool> HasInventoryAvailableAsync(IEnumerable<OrderProduct> orderProducts)
    {
        foreach (var orderProduct in orderProducts)
        {

            var product =
                await _unitOfWork.ProductRepository.FindFirstSatisfyingAsync(new QueryActiveProductByIdSpecification(orderProduct.ProductId))
                ?? throw new ProductNotFoundException($"It was not possible to verify inventory availability. The product with id {orderProduct.ProductId} was not found");

            if (!product.Inventory.HasInventoryAvailable(orderProduct.Quantity))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<decimal> CalculateTotalAsync(IEnumerable<OrderProduct> orderProducts, IEnumerable<OrderCoupon>? couponsApplied)
    {
        var productsWithPrices = await CalculateProductPricesAsync(orderProducts);

        var total = productsWithPrices.Sum(p => p.ProductPrice * p.QuantityReserved);

        if (couponsApplied == null || !couponsApplied.Any())
        {
            return total;
        }

        var totalAfterCouponDiscounts = await ApplyCouponsAsync(productsWithPrices.Select(p => p.Product), couponsApplied, total);

        return totalAfterCouponDiscounts;
    }

    private async Task<List<(decimal ProductPrice, int QuantityReserved, Product Product)>> CalculateProductPricesAsync(
        IEnumerable<OrderProduct> orderProducts
    )
    {
        var products = await Task.WhenAll(orderProducts.Select(async orderProduct =>
        {
            var product =
                await _unitOfWork.ProductRepository.FindFirstSatisfyingAsync(new QueryActiveProductByIdSpecification(orderProduct.ProductId))
                ?? throw new ProductNotFoundException($"Product with id {orderProduct.ProductId} not found");

            var productPrice = await _productService.CalculateProductPriceApplyingSaleAsync(product);

            return (
                ProductPrice: productPrice,
                QuantityReserved: orderProduct.Quantity,
                Product: product
           );
        }));

        return products.ToList();
    }

    private async Task<decimal> ApplyCouponsAsync(
        IEnumerable<Product> products,
        IEnumerable<OrderCoupon> couponsApplied,
        decimal total
    )
    {
        var productsWithCategories = products
            .Select(p => CouponOrderProduct.Create(
                p.Id,
                p.ProductCategories.Select(c => c.CategoryId)))
            .ToList();

        var couponDiscountsToApply = new List<Discount>();

        foreach (var orderCouponApplied in couponsApplied)
        {
            var coupon = await _unitOfWork.CouponRepository.FindByIdAsync(orderCouponApplied.CouponId);

            if (coupon == null || !await _couponService.IsApplicableAsync(coupon, CouponOrder.Create(productsWithCategories, total)))
            {
                throw new InvalidOrderCouponAppliedException("Some of the coupons are expired or invalid");
            }

            couponDiscountsToApply.Add(coupon.Discount);
        }

        return DiscountService.ApplyDiscounts(total, [.. couponDiscountsToApply]);
    }
}
