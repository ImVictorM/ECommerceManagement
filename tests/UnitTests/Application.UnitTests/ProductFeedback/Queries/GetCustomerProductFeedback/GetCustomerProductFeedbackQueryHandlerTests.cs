using Application.Common.Persistence.Repositories;
using Application.ProductFeedback.DTOs;
using Application.ProductFeedback.Queries.GetCustomerProductFeedback;
using Application.UnitTests.ProductFeedback.Queries.TestUtils;

using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using SharedKernel.Interfaces;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ProductFeedback.Queries.GetCustomerProductFeedback;

/// <summary>
/// Unit tests for the <see cref="GetCustomerProductFeedbackQueryHandler"/> handler.
/// </summary>
public class GetCustomerProductFeedbackQueryHandlerTests
{
    private readonly Mock<IProductFeedbackRepository> _mockProductFeedbackRepository;
    private readonly GetCustomerProductFeedbackQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetCustomerProductFeedbackQueryHandlerTests"/> class.
    /// </summary>
    public GetCustomerProductFeedbackQueryHandlerTests()
    {
        _mockProductFeedbackRepository = new Mock<IProductFeedbackRepository>();

        _handler = new GetCustomerProductFeedbackQueryHandler(
            _mockProductFeedbackRepository.Object,
            Mock.Of<ILogger<GetCustomerProductFeedbackQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies when feedback exists for a customer, it is returned.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerProductFeedback_WhenFeedbackExists_ReturnsFeedbackResults()
    {
        var query = GetCustomerProductFeedbackQueryUtils.CreateQuery(userId: "1");
        var userId = UserId.Create(query.UserId);

        var feedbackResults = new List<ProductFeedbackResult>
        {
            new(
                ProductFeedbackUtils.CreateProductFeedback(
                    productId: ProductId.Create(1),
                    title: "Feedback 1",
                    content: "Feedback 1 content",
                    userId: userId
                )
            ),
            new(
                ProductFeedbackUtils.CreateProductFeedback(
                    productId: ProductId.Create(2),
                    title: "Feedback 2",
                    content: "Feedback 2 content",
                    userId: userId
                )
            ),
        };

        _mockProductFeedbackRepository
            .Setup(r => r.GetProductFeedbackSatisfyingAsync(
                It.IsAny<ISpecificationQuery<DomainProductFeedback>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(feedbackResults);

        var result = await _handler.Handle(query, default);

        result.Should().BeEquivalentTo(feedbackResults);
    }

    /// <summary>
    /// Verifies when no feedback exists for a customer, an empty collection is returned.
    /// </summary>
    [Fact]
    public async Task HandleGetCustomerProductFeedback_WhenNoFeedbackExists_ReturnsEmptyCollection()
    {
        var query = GetCustomerProductFeedbackQueryUtils.CreateQuery(userId: "2");

        _mockProductFeedbackRepository
            .Setup(r => r.GetProductFeedbackSatisfyingAsync(
                It.IsAny<ISpecificationQuery<DomainProductFeedback>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync([]);

        var result = await _handler.Handle(query, default);

        result.Should().BeEmpty();
    }
}
