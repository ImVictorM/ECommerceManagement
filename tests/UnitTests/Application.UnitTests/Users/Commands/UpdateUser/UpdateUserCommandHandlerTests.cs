using System.Linq.Expressions;
using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.UnitTests.Users.Commands.TestUtils;
using Application.Users.Commands.UpdateUser;
using Application.Users.Common.Errors;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Users.Commands.UpdateUser;

/// <summary>
/// Contains tests for the <see cref="UpdateUserCommandHandler"/> to validate correct handling of user updates.
/// </summary>
public class UpdateUserCommandHandlerTests
{
    private readonly UpdateUserCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnityOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateUserCommandHandlerTests"/> class.
    /// </summary>
    public UpdateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockUnityOfWork = new Mock<IUnitOfWork>();

        _mockUnityOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new UpdateUserCommandHandler(
            _mockUnityOfWork.Object,
            new Mock<ILogger<UpdateUserCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests that when a user exists, the handler correctly updates the user's information.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUser_WhenUserExists_UpdateTheUserCorrectly()
    {
        var userToBeUpdated = UserUtils.CreateUser();

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<UserId>()))
            .ReturnsAsync(userToBeUpdated);

        _mockUserRepository
            .Setup(r => r.FindOneOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);

        var command = UpdateUserCommandUtils.CreateCommand(name: "new name", phone: "19958274823");

        await _handler.Handle(command, default);

        userToBeUpdated.Name.Should().Be(command.Name);
        userToBeUpdated.Phone.Should().Be(command.Phone);

        _mockUserRepository.Verify(r => r.UpdateAsync(userToBeUpdated), Times.Once());
        _mockUnityOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Tests that when a user does not exist, the handler throws a <see cref="UserNotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUser_WhenUserDoesNotExist_ThrowsException()
    {
        _mockUserRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<UserId>()))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(UpdateUserCommandUtils.CreateCommand(), default))
            .Should()
            .ThrowAsync<UserNotFoundException>()
            .WithMessage("The user to be updated does not exist");
    }

    /// <summary>
    /// Tests that when the user being updated is inactive an error is thrown.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUser_WhenUserIsInactive_ThrowsException()
    {
        var inactiveUser = UserUtils.CreateUser(active: false);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<UserId>()))
            .ReturnsAsync(inactiveUser);

        await FluentActions
            .Invoking(() => _handler.Handle(UpdateUserCommandUtils.CreateCommand(), default))
            .Should()
            .ThrowAsync<UserNotFoundException>()
            .WithMessage("The user to be updated does not exist");
    }

    /// <summary>
    /// Tests that when trying to update a user's email to an already existing one, 
    /// the handler throws a <see cref="UserAlreadyExistsException"/>.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUser_WhenTryingToUpdateEmailWithExistingOne_ThrowsException()
    {
        var userToBeUpdatedNewEmail = "existing_email@email.com";
        var userToBeUpdated = UserUtils.CreateUser();
        var conflictingUser = UserUtils.CreateUser(email: userToBeUpdatedNewEmail);
        var updateRequest = UpdateUserCommandUtils.CreateCommand(email: userToBeUpdatedNewEmail);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<UserId>()))
            .ReturnsAsync(userToBeUpdated);

        _mockUserRepository
            .Setup(r => r.FindOneOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(conflictingUser);

        await FluentActions
            .Invoking(() => _handler.Handle(updateRequest, default))
            .Should()
            .ThrowAsync<UserAlreadyExistsException>()
            .WithMessage("The email you entered is already in use");
    }
}
