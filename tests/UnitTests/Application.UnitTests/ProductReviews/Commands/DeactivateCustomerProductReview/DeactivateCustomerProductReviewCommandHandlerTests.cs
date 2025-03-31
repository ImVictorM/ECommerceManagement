using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.ProductReviews.Errors;
using Application.ProductReviews.Commands.DeactivateCustomerProductReview;
using Application.UnitTests.ProductReviews.Commands.TestUtils;

using Domain.ProductReviewAggregate.ValueObjects;
using Domain.ProductReviewAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ProductReviews.Commands.DeactivateCustomerProductReview;

/// <summary>
/// Unit tests for <see cref="DeactivateCustomerProductReviewCommandHandler"/>
/// handler.
/// </summary>
public class DeactivateCustomerProductReviewCommandHandlerTests
{
    private readonly Mock<IProductReviewRepository> _mockProductReviewRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeactivateCustomerProductReviewCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DeactivateCustomerProductReviewCommandHandlerTests"/> class.
    /// </summary>
    public DeactivateCustomerProductReviewCommandHandlerTests()
    {
        _mockProductReviewRepository = new Mock<IProductReviewRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new DeactivateCustomerProductReviewCommandHandler(
            _mockProductReviewRepository.Object,
            _mockUnitOfWork.Object,
            Mock.Of<ILogger<DeactivateCustomerProductReviewCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies that when the review exists, it is deactivated successfully.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateCustomerProductReviewCommand_WithExistentReview_DeactivatesSuccessfully()
    {
        var command = DeactivateCustomerProductReviewCommandUtils.CreateCommand();

        var reviewId = ProductReviewId.Create(command.ReviewId);
        var review = ProductReviewUtils.CreateProductReview(id: reviewId);

        _mockProductReviewRepository
            .Setup(r => r.FindByIdAsync(
                reviewId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(review);

        await _handler.Handle(command, default);

        review.IsActive.Should().BeFalse();
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies that when the review does not exist, an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateCustomerProductReviewCommand_WithNonExistentReview_ThrowsError()
    {
        var command = DeactivateCustomerProductReviewCommandUtils.CreateCommand();
        var reviewId = ProductReviewId.Create(command.ReviewId);

        _mockProductReviewRepository
            .Setup(r => r.FindByIdAsync(
                reviewId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((ProductReview?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ProductReviewNotFoundException>();

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
}
