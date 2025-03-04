using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.ProductFeedback.Commands.DeactivateCustomerProductFeedback;
using Application.UnitTests.ProductFeedback.Commands.TestUtils;
using Application.ProductFeedback.Errors;

using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ProductFeedback.Commands.DeactivateCustomerProductFeedback;

/// <summary>
/// Unit tests for <see cref="DeactivateCustomerProductFeedbackCommandHandler"/>.
/// </summary>
public class DeactivateCustomerProductFeedbackCommandHandlerTests
{
    private readonly Mock<IProductFeedbackRepository> _mockProductFeedbackRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeactivateCustomerProductFeedbackCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DeactivateCustomerProductFeedbackCommandHandlerTests"/> class.
    /// </summary>
    public DeactivateCustomerProductFeedbackCommandHandlerTests()
    {
        _mockProductFeedbackRepository = new Mock<IProductFeedbackRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new DeactivateCustomerProductFeedbackCommandHandler(
            _mockProductFeedbackRepository.Object,
            _mockUnitOfWork.Object,
            Mock.Of<ILogger<DeactivateCustomerProductFeedbackCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies that when feedback exists, it is deactivated successfully.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateCustomerProductFeedback_WhenFeedbackExists_DeactivatesSuccessfully()
    {
        var command = DeactivateCustomerProductFeedbackCommandUtils.CreateCommand();

        var feedbackId = ProductFeedbackId.Create(command.FeedbackId);
        var feedback = ProductFeedbackUtils.CreateProductFeedback(id: feedbackId);

        _mockProductFeedbackRepository
            .Setup(r => r.FindByIdAsync(
                feedbackId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(feedback);

        await _handler.Handle(command, default);

        feedback.IsActive.Should().BeFalse();
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies that when feedback does not exist, an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateCustomerProductFeedback_WhenFeedbackDoesNotExist_ThrowsError()
    {
        var command = DeactivateCustomerProductFeedbackCommandUtils.CreateCommand();
        var feedbackId = ProductFeedbackId.Create(command.FeedbackId);

        _mockProductFeedbackRepository
            .Setup(r => r.FindByIdAsync(
                feedbackId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((DomainProductFeedback?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ProductFeedbackNotFoundException>();

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
}
