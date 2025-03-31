using Application.Common.Persistence.Repositories;
using Application.ProductReviews.Queries.GetCustomerProductReviews;
using Application.UnitTests.ProductReviews.Queries.TestUtils;
using Application.UnitTests.ProductReviews.TestUtils.Extensions;

using Domain.ProductReviewAggregate;
using Domain.ProductReviewAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;

using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.ProductReviews.Queries.GetCustomerProductReviews;

/// <summary>
/// Unit tests for the <see cref="GetCustomerProductReviewsQueryHandler"/> handler.
/// </summary>
public class GetCustomerProductReviewsQueryHandlerTests
{
    private readonly Mock<IProductReviewRepository> _mockProductReviewRepository;
    private readonly GetCustomerProductReviewsQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetCustomerProductReviewsQueryHandlerTests"/> class.
    /// </summary>
    public GetCustomerProductReviewsQueryHandlerTests()
    {
        _mockProductReviewRepository = new Mock<IProductReviewRepository>();

        _handler = new GetCustomerProductReviewsQueryHandler(
            _mockProductReviewRepository.Object,
            Mock.Of<ILogger<GetCustomerProductReviewsQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies the reviews are retrieved correctly.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerProductReviewsQuery_WithExistentReviews_ReturnsReviews()
    {
        var query = GetCustomerProductReviewsQueryUtils.CreateQuery(userId: "1");
        var userId = UserId.Create(query.UserId);

        var reviews = new List<ProductReview>
        {
            ProductReviewUtils.CreateProductReview(
                id: ProductReviewId.Create(1),
                productId: ProductId.Create(1),
                title: "Review 1",
                content: "Review 1 content",
                userId: userId
            ),
            ProductReviewUtils.CreateProductReview(
                id: ProductReviewId.Create(2),
                productId: ProductId.Create(2),
                title: "Review 2",
                content: "Review 2 content",
                userId: userId
            )
        };

        _mockProductReviewRepository
            .Setup(r => r.FindSatisfyingAsync(
                It.IsAny<ISpecificationQuery<ProductReview>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(reviews);

        var result = await _handler.Handle(query, default);

        result.EnsureCorrespondsTo(reviews);
    }
}
