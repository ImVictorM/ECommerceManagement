using Application.Common.Persistence;
using Application.UnitTests.Users.Commands.TestUtils;
using Application.Users.Commands.DeactivateUser;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Users.Commands.DeactivateUser;

/// <summary>
/// Unit tests for the <see cref="DeactivateUserCommandHandler"/> handler.
/// </summary>
public class DeactivateUserCommandHandlerTests
{
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeactivateUserCommandHandler _handler;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserCommandHandlerTests"/> class.
    /// </summary>
    public DeactivateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new DeactivateUserCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<DeactivateUserCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests that when the user to be deactivated exists it is deactivated.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateUser_WhenUserExists_DeactivateItAndSave()
    {
        var userToBeDeactivated = UserUtils.CreateUser(active: true);

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(userToBeDeactivated);

        await _handler.Handle(DeactivateUserCommandUtils.CreateCommand(), default);

        userToBeDeactivated.IsActive.Should().BeFalse();
        _mockUnitOfWork.Verify(uof => uof.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Tests when a user to be deactivated does not exist the command handler completes without throwing any exceptions.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateUser_WhenUserDoesNotExist_ReturnsWithoutThrowing()
    {
        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(DeactivateUserCommandUtils.CreateCommand(), default))
            .Should()
            .NotThrowAsync();

        _mockUnitOfWork.Verify(uof => uof.SaveChangesAsync(), Times.Never());
    }
}
