using Application.Common.Persistence.Repositories;
using Application.ProductFeedback.Services;

using Domain.OrderAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ProductFeedback.Services;

/// <summary>
/// Unit tests for the <see cref="ProductFeedbackEligibilityService"/> service.
/// </summary>
public class ProductFeedbackEligibilityServiceTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly ProductFeedbackEligibilityService _service;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="ProductFeedbackEligibilityServiceTests"/> class.
    /// </summary>
    public ProductFeedbackEligibilityServiceTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _service = new ProductFeedbackEligibilityService(
            _mockOrderRepository.Object
        );
    }

    /// <summary>
    /// Verifies the method returns true when the user has purchased the product.
    /// </summary>
    [Fact]
    public async Task CanLeaveFeedbackAsync_WhenUserHasPurchasedProduct_ReturnsTrue()
    {
        var userId = UserId.Create(1);
        var productId = ProductId.Create(1);

        var order = await OrderUtils.CreateOrderAsync(
            ownerId: userId,
            orderLineItemDrafts:
            [
                OrderUtils.CreateOrderLineItemDraft(productId: productId)
            ]
        );

        _mockOrderRepository
            .Setup(repo => repo.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(order);

        var result = await _service.CanLeaveFeedbackAsync(userId, productId);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the method returns false when the user has not purchased the product.
    /// </summary>
    [Fact]
    public async Task CanLeaveFeedbackAsync_WhenUserHasNotPurchasedProduct_ReturnsFalse()
    {
        var userId = UserId.Create(1);
        var productId = ProductId.Create(1);

        _mockOrderRepository
            .Setup(repo => repo.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Order?)null);

        var result = await _service.CanLeaveFeedbackAsync(userId, productId);

        result.Should().BeFalse();
    }
}
