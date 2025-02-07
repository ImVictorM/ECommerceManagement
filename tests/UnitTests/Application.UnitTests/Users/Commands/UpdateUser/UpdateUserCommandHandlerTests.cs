using Application.Common.Persistence;
using Application.UnitTests.Users.Commands.TestUtils;
using Application.Users.Errors;
using Application.Users.Commands.UpdateUser;
using Application.Common.Errors;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Users.Commands.UpdateUser;

/// <summary>
/// Unit tests for the <see cref="UpdateUserCommandHandler"/> handler.
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
    /// Tests that when the user to be updated exists they are updated.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUser_WhenUserExists_UpdateTheUserCorrectly()
    {
        var userToBeUpdated = UserUtils.CreateCustomer();
        var command = UpdateUserCommandUtils.CreateCommand(name: "new name", phone: "19958274823");

        _mockUserRepository
            .SetupSequence(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(userToBeUpdated);

        await _handler.Handle(command, default);

        userToBeUpdated.Name.Should().Be(command.Name);
        userToBeUpdated.Phone.Should().Be(command.Phone);

        _mockUnityOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Tests that when a user does not exist, the handler throws a <see cref="UserNotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUser_WhenUserDoesNotExist_ThrowsException()
    {
        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(UpdateUserCommandUtils.CreateCommand(), default))
            .Should()
            .ThrowAsync<UserNotFoundException>()
            .WithMessage("The user to be updated could not be found");
    }

    /// <summary>
    /// Tests that when trying to update a user's email to an already existing one,
    /// the handler throws a <see cref="EmailConflictException"/>.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUser_WhenTryingToUpdateEmailWithExistingOne_ThrowsException()
    {
        var userToBeUpdatedNewEmail = EmailUtils.CreateEmail("existing_email@email.com");

        var userToBeUpdated = UserUtils.CreateCustomer();
        var conflictingUser = UserUtils.CreateCustomer(email: userToBeUpdatedNewEmail);
        var updateRequest = UpdateUserCommandUtils.CreateCommand(email: userToBeUpdatedNewEmail.ToString());

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(userToBeUpdated);

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryUserByEmailSpecification>()))
            .ReturnsAsync(conflictingUser);

        await FluentActions
            .Invoking(() => _handler.Handle(updateRequest, default))
            .Should()
            .ThrowAsync<EmailConflictException>();
    }
}
