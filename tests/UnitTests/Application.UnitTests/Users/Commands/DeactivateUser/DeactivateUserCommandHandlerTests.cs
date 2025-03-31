using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Users.Commands.DeactivateUser;
using Application.Users.Errors;
using Application.UnitTests.Users.Commands.TestUtils;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Users.Commands.DeactivateUser;

/// <summary>
/// Unit tests for the <see cref="DeactivateUserCommandHandler"/> handler.
/// </summary>
public class DeactivateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeactivateUserCommandHandler _handler;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="DeactivateUserCommandHandlerTests"/> class.
    /// </summary>
    public DeactivateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new DeactivateUserCommandHandler(
            _mockUnitOfWork.Object,
            _mockUserRepository.Object,
            Mock.Of<ILogger<DeactivateUserCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies that when the user to be deactivated exists it is deactivated.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateUserCommand_WithExistentUser_DeactivatesTheUser()
    {
        var userToBeDeactivated = UserUtils.CreateCustomer(active: true);

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveUserByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(userToBeDeactivated);

        await _handler.Handle(DeactivateUserCommandUtils.CreateCommand(), default);

        userToBeDeactivated.IsActive.Should().BeFalse();
        _mockUnitOfWork.Verify(uof => uof.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies an exception is thrown when the user does not exist.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateUserCommand_WhenUserDoesNotExist_ThrowsError()
    {
        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveUserByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(
                DeactivateUserCommandUtils.CreateCommand(),
                default
            ))
            .Should()
            .ThrowAsync<UserNotFoundException>();

        _mockUnitOfWork.Verify(uof => uof.SaveChangesAsync(), Times.Never);
    }
}
