using Application.Common.Interfaces.Persistence;
using Application.UnitTests.Users.Commands.TestUtils;
using Application.Users.Commands.DeactivateUser;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Users.Commands.DeactivateUser;

/// <summary>
/// Contains tests for the <see cref="DeactivateUserCommandHandler"/> to validate correct handling of user deactivation.
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

        _handler = new DeactivateUserCommandHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests that when a user exists, the command handler deactivates the user and saves the changes.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateUser_WhenUserExists_DeactivateItAndSave()
    {
        var userToBeDeactivated = UserUtils.CreateUser(active: true);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<UserId>()))
            .ReturnsAsync(userToBeDeactivated);

        await _handler.Handle(DeactivateUserCommandUtils.CreateCommand(), default);

        userToBeDeactivated.IsActive.Should().BeFalse();
        _mockUnitOfWork.Verify(uof => uof.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Tests when a user does not exist, the command handler completes without throwing any exceptions.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateUser_WhenUserDoesNotExist_ReturnsWithoutThrowing()
    {
        _mockUserRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<UserId>()))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(DeactivateUserCommandUtils.CreateCommand(), default))
            .Should()
            .NotThrowAsync();

        _mockUnitOfWork.Verify(uof => uof.SaveChangesAsync(), Times.Never());
    }
}
