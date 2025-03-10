using Application.Common.Persistence.Repositories;

using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.Services;
using Domain.UserAggregate.ValueObjects;

namespace Application.ProductFeedback.Services;

internal sealed class ProductFeedbackEligibilityService
    : IProductFeedbackEligibilityService
{
    private readonly IOrderRepository _orderRepository;

    public ProductFeedbackEligibilityService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task<bool> CanLeaveFeedbackAsync(
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
