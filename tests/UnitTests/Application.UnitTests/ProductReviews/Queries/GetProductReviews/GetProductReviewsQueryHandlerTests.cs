using Application.ProductReviews.Queries.GetProductReviews;
using Application.Common.Persistence.Repositories;
using Application.UnitTests.ProductReviews.TestUtils.Projections;
using Application.UnitTests.ProductReviews.TestUtils.Extensions;
using Application.UnitTests.ProductReviews.Queries.TestUtils;

using Domain.ProductReviewAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;

using Microsoft.Extensions.Logging;
using Moq;
using Domain.ProductReviewAggregate.ValueObjects;

namespace Application.UnitTests.ProductReviews.Queries.GetProductReviews;

/// <summary>
/// Unit tests for the <see cref="GetProductReviewsQueryHandler"/> handler.
/// </summary>
public class GetProductReviewsQueryHandlerTests
{
    private readonly Mock<IProductReviewRepository> _mockProductReviewRepository;
    private readonly GetProductReviewsQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetProductReviewsQueryHandlerTests"/> class.
    /// </summary>
    public GetProductReviewsQueryHandlerTests()
    {
        _mockProductReviewRepository = new Mock<IProductReviewRepository>();

        _handler = new GetProductReviewsQueryHandler(
            _mockProductReviewRepository.Object,
            Mock.Of<ILogger<GetProductReviewsQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies the reviews are retrieved correctly.
    /// </summary>
    [Fact]
    public async Task HandleGetProductReviewsQuery_WithExistentReviews_ReturnsReviews()
    {
        var query = GetProductReviewsQueryUtils.CreateQuery(productId: "1");
        var productId = ProductId.Create(query.ProductId);

        var usersWithReviews = new List<User>
        {
            UserUtils.CreateCustomer(id: UserId.Create(1)),
            UserUtils.CreateCustomer(id: UserId.Create(2)),
        };

        var reviews = new List<ProductReview>
        {
            ProductReviewUtils.CreateProductReview(
                id: ProductReviewId.Create(1),
                productId: productId,
                title: "Review 1",
                content: "Review 1 content",
                userId: usersWithReviews[0].Id
            ),
            ProductReviewUtils.CreateProductReview(
                id: ProductReviewId.Create(2),
                productId: productId,
                title: "Review 1",
                content: "Review 1 content",
                userId: usersWithReviews[1].Id
            ),
        };

        var projections = reviews
            .Zip(
                usersWithReviews,
                ProductReviewDetailedProjectionUtils.CreateProjection
            )
            .ToList();

        _mockProductReviewRepository
            .Setup(r => r.GetProductReviewsDetailedSatisfyingAsync(
                It.IsAny<ISpecificationQuery<ProductReview>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(projections);

        var result = await _handler.Handle(query, default);

        result.EnsureCorrespondsTo(projections);
    }
}
