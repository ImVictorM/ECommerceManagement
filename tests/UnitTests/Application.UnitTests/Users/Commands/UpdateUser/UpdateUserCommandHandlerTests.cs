using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.Errors;
using Application.UnitTests.Users.Commands.TestUtils;
using Application.Users.Errors;
using Application.Users.Commands.UpdateUser;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;

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
    private readonly Mock<IUserRepository> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="UpdateUserCommandHandlerTests"/> class.
    /// </summary>
    public UpdateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnityOfWork = new Mock<IUnitOfWork>();

        _handler = new UpdateUserCommandHandler(
            _mockUnityOfWork.Object,
            _mockUserRepository.Object,
            Mock.Of<ILogger<UpdateUserCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies that when the user to be updated exists they are updated.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUserCommand_WhenUserExists_UpdateTheUserCorrectly()
    {
        var userToBeUpdated = UserUtils.CreateCustomer();
        var command = UpdateUserCommandUtils.CreateCommand(
            name: "new name",
            phone: "19958274823"
        );

        _mockUserRepository
            .SetupSequence(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveUserByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(userToBeUpdated);

        await _handler.Handle(command, default);

        userToBeUpdated.Name.Should().Be(command.Name);
        userToBeUpdated.Phone.Should().Be(command.Phone);

        _mockUnityOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies an exception is thrown when the user does not exist.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUserCommand_WhenUserDoesNotExist_ThrowsException()
    {
        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveUserByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(
                UpdateUserCommandUtils.CreateCommand(),
                default
            ))
            .Should()
            .ThrowAsync<UserNotFoundException>();
    }

    /// <summary>
    /// Verifies that when trying to update the user's email with an already existing one,
    /// the handler throws a <see cref="EmailConflictException"/>.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUserCommand_WithConflictingEmail_ThrowsException()
    {
        var userToBeUpdatedNewEmail = EmailUtils.CreateEmail(
            "existent_email@email.com"
        );

        var userToBeUpdated = UserUtils.CreateCustomer();
        var conflictingUser = UserUtils.CreateCustomer(
            email: userToBeUpdatedNewEmail
        );
        var updateRequest = UpdateUserCommandUtils.CreateCommand(
            email: userToBeUpdatedNewEmail.ToString()
        );

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveUserByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(userToBeUpdated);

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryUserByEmailSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(conflictingUser);

        await FluentActions
            .Invoking(() => _handler.Handle(updateRequest, default))
            .Should()
            .ThrowAsync<EmailConflictException>();
    }
}
