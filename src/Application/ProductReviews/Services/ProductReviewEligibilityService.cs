using Application.Common.Persistence.Repositories;

using Domain.ProductAggregate.ValueObjects;
using Domain.ProductReviewAggregate.Services;
using Domain.UserAggregate.ValueObjects;

namespace Application.ProductReviews.Services;

internal sealed class ProductReviewEligibilityService
    : IProductReviewEligibilityService
{
    private readonly IOrderRepository _orderRepository;

    public ProductReviewEligibilityService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> CanLeaveReviewAsync(
        UserId userId,
        ProductId productId,
        CancellationToken cancellationToken = default
    )
    {
        var userOrderContainingProduct = await _orderRepository.FirstOrDefaultAsync(o =>
            o.OwnerId == userId &&
            o.Products.Select(o => o.ProductId).Contains(productId),
            cancellationToken
        );

        return userOrderContainingProduct != null;
    }
}
