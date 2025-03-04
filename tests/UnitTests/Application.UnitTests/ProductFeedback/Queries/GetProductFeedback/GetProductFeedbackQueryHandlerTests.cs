using Application.Common.Persistence.Repositories;
using Application.ProductFeedback.DTOs;
using Application.ProductFeedback.Queries.GetProductFeedback;
using Application.UnitTests.ProductFeedback.Queries.TestUtils;

using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using SharedKernel.Interfaces;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.ProductFeedback.Queries.GetProductFeedback;

/// <summary>
/// Unit tests for the <see cref="GetProductFeedbackQueryHandler"/> handler.
/// </summary>
public class GetProductFeedbackQueryHandlerTests
{
    private readonly Mock<IProductFeedbackRepository> _mockProductFeedbackRepository;
    private readonly GetProductFeedbackQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductFeedbackQueryHandlerTests"/> class.
    /// </summary>
    public GetProductFeedbackQueryHandlerTests()
    {
        _mockProductFeedbackRepository = new Mock<IProductFeedbackRepository>();

        _handler = new GetProductFeedbackQueryHandler(
            _mockProductFeedbackRepository.Object,
            Mock.Of<ILogger<GetProductFeedbackQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies when feedback exists for a product, it is returned.
    /// </summary>
    [Fact]
    public async Task HandleGetProductFeedback_WhenFeedbackExists_ReturnsFeedbackResults()
    {
        var query = GetProductFeedbackQueryUtils.CreateQuery(productId: "1");
        var productId = ProductId.Create(query.ProductId);

        var feedbackResults = new List<ProductFeedbackDetailedResult>
        {
            new(
                ProductFeedbackUtils.CreateProductFeedback(
                    productId: productId,
                    title: "Feedback 1",
                    content: "Feedback 1 content"
                ),
                new ProductFeedbackUserResult(
                    UserId.Create(1), "User 1"
                )
            ),
            new(
                ProductFeedbackUtils.CreateProductFeedback(
                    productId: productId,
                    title: "Feedback 2",
                    content: "Feedback 2 content"
                ),
                new ProductFeedbackUserResult(
                    UserId.Create(2), "User 2"
                )
            ),
        };

        _mockProductFeedbackRepository
            .Setup(r => r.GetProductFeedbackDetailedSatisfyingAsync(
                It.IsAny<ISpecificationQuery<DomainProductFeedback>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(feedbackResults);

        var result = await _handler.Handle(query, default);

        result.Should().BeEquivalentTo(feedbackResults);
    }

    /// <summary>
    /// Verifies when no feedback exists for a product, an empty collection is returned.
    /// </summary>
    [Fact]
    public async Task HandleGetProductFeedback_WhenNoFeedbackExists_ReturnsEmptyCollection()
    {
        var query = GetProductFeedbackQueryUtils.CreateQuery(productId: "2");

        _mockProductFeedbackRepository
            .Setup(r => r.GetProductFeedbackDetailedSatisfyingAsync(
                It.IsAny<ISpecificationQuery<DomainProductFeedback>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync([]);

        var result = await _handler.Handle(query, default);

        result.Should().BeEmpty();
    }
}
