using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.ProductReviews.Errors;
using Application.ProductReviews.Commands.LeaveProductReview;
using Application.UnitTests.ProductReviews.Commands.TestUtils;
using Application.UnitTests.TestUtils.Extensions;

using Domain.ProductReviewAggregate.ValueObjects;
using Domain.ProductReviewAggregate;
using Domain.ProductReviewAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ProductReviews.Commands.LeaveProductReview;

/// <summary>
/// Unit tests for the <see cref="LeaveProductReviewCommandHandler"/>
/// command handler.
/// </summary>
public class LeaveProductReviewCommandHandlerTests
{
    private readonly Mock<IProductReviewRepository> _mockProductReviewRepository;
    private readonly Mock<IProductReviewEligibilityService> _mockEligibilityService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IIdentityProvider> _mockIdentityProvider;
    private readonly LeaveProductReviewCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="LeaveProductReviewCommandHandlerTests"/> class.
    /// </summary>
    public LeaveProductReviewCommandHandlerTests()
    {
        _mockProductReviewRepository = new Mock<IProductReviewRepository>();
        _mockEligibilityService = new Mock<IProductReviewEligibilityService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockIdentityProvider = new Mock<IIdentityProvider>();

        _handler = new LeaveProductReviewCommandHandler(
            _mockProductReviewRepository.Object,
            _mockEligibilityService.Object,
            _mockUnitOfWork.Object,
            _mockIdentityProvider.Object,
            Mock.Of<ILogger<LeaveProductReviewCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies a review is created successfully when the user has permission.
    /// </summary>
    [Fact]
    public async Task HandleLeaveProductReviewCommand_WithAllowedUser_ShouldLeaveReview()
    {
        var command = LeaveProductReviewCommandUtils.CreateCommand();
        var currentUserIdentity = new IdentityUser("1", [Role.Customer]);
        var currentUserId = UserId.Create(currentUserIdentity.Id);
        var productId = ProductId.Create(command.ProductId);
        var createdReviewId = ProductReviewId.Create(10);

        _mockIdentityProvider
            .Setup(p => p.GetCurrentUserIdentity())
            .Returns(currentUserIdentity);

        _mockEligibilityService
            .Setup(s => s.CanLeaveReviewAsync(
                currentUserId,
                productId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        _mockUnitOfWork.MockSetEntityIdBehavior<
            IProductReviewRepository,
            ProductReview,
            ProductReviewId
        >(
            _mockProductReviewRepository,
            createdReviewId
        );

        var result = await _handler.Handle(command, default);

        result.Id.Should().Be(createdReviewId.ToString());

        _mockProductReviewRepository.Verify(r =>
            r.AddAsync(It.Is<ProductReview>(f => f.ProductId == productId)),
            Times.Once()
        );
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies that an exception is thrown when the user cannot leave a review.
    /// </summary>
    [Fact]
    public async Task HandleLeaveProductReviewCommand_WithoutPermission_ThrowsException()
    {
        var command = LeaveProductReviewCommandUtils.CreateCommand();
        var currentUserIdentity = new IdentityUser("1", [Role.Customer]);
        var userId = UserId.Create(currentUserIdentity.Id);
        var productId = ProductId.Create(1);

        _mockIdentityProvider
            .Setup(p => p.GetCurrentUserIdentity())
            .Returns(currentUserIdentity);

        _mockEligibilityService
            .Setup(s => s.CanLeaveReviewAsync(
                userId,
                productId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(false);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<UserCannotLeaveReviewException>();
    }
}
