using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.ProductFeedback.Commands.LeaveProductFeedback;
using Application.ProductFeedback.Errors;
using Application.UnitTests.TestUtils.Extensions;
using Application.UnitTests.ProductFeedback.Commands.TestUtils;

using Domain.UserAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;
using Domain.ProductFeedbackAggregate.ValueObjects;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ProductFeedback.Commands.LeaveProductFeedback;

/// <summary>
/// Unit tests for the <see cref="LeaveProductFeedbackCommandHandler"/> command handler.
/// </summary>
public class LeaveProductFeedbackCommandHandlerTests
{
    private readonly Mock<IProductFeedbackRepository> _mockProductFeedbackRepository;
    private readonly Mock<IProductFeedbackService> _mockProductFeedbackService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IIdentityProvider> _mockIdentityProvider;
    private readonly LeaveProductFeedbackCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="LeaveProductFeedbackCommandHandlerTests"/> class.
    /// </summary>
    public LeaveProductFeedbackCommandHandlerTests()
    {
        _mockProductFeedbackRepository = new Mock<IProductFeedbackRepository>();
        _mockProductFeedbackService = new Mock<IProductFeedbackService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockIdentityProvider = new Mock<IIdentityProvider>();
        _handler = new LeaveProductFeedbackCommandHandler(
            _mockProductFeedbackRepository.Object,
            _mockProductFeedbackService.Object,
            _mockUnitOfWork.Object,
            _mockIdentityProvider.Object,
            Mock.Of<ILogger<LeaveProductFeedbackCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies that feedback is created successfully when the user has permission.
    /// </summary>
    [Fact]
    public async Task HandleLeaveProductFeedback_WhenUserCanLeaveFeedback_CreatesFeedbackSuccessfully()
    {
        var command = LeaveProductFeedbackCommandUtils.CreateCommand();
        var currentUserIdentity = new IdentityUser("1", [Role.Customer]);
        var currentUserId = UserId.Create(currentUserIdentity.Id);
        var productId = ProductId.Create(command.ProductId);
        var createdProductFeedbackId = ProductFeedbackId.Create(10);

        _mockIdentityProvider
            .Setup(p => p.GetCurrentUserIdentity())
            .Returns(currentUserIdentity);

        _mockProductFeedbackService
            .Setup(s => s.CanLeaveFeedbackAsync(
                currentUserId,
                productId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        _mockUnitOfWork.MockSetEntityIdBehavior<
            IProductFeedbackRepository,
            DomainProductFeedback,
            ProductFeedbackId
        >(
            _mockProductFeedbackRepository,
            createdProductFeedbackId
        );

        var result = await _handler.Handle(command, default);

        result.Id.Should().Be(createdProductFeedbackId.ToString());

        _mockProductFeedbackRepository.Verify(r =>
            r.AddAsync(It.Is<DomainProductFeedback>(f => f.ProductId == productId)),
            Times.Once()
        );
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies that an exception is thrown when the user cannot leave feedback.
    /// </summary>
    [Fact]
    public async Task HandleLeaveProductFeedback_WhenUserCannotLeaveFeedback_ThrowsException()
    {
        var command = LeaveProductFeedbackCommandUtils.CreateCommand();
        var currentUserIdentity = new IdentityUser("1", [Role.Customer]);
        var userId = UserId.Create(currentUserIdentity.Id);
        var productId = ProductId.Create(1);

        _mockIdentityProvider
            .Setup(p => p.GetCurrentUserIdentity())
            .Returns(currentUserIdentity);

        _mockProductFeedbackService
            .Setup(s => s.CanLeaveFeedbackAsync(
                userId,
                productId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(false);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<UserCannotLeaveFeedbackException>();
    }
}
