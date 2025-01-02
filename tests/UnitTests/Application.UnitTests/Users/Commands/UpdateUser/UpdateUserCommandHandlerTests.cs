using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.UnitTests.Users.Commands.TestUtils;
using Application.Users.Commands.UpdateUser;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Authorization;
using SharedKernel.UnitTests.TestUtils;

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
    /// List of not allowed pairs.
    /// </summary>
    public static IEnumerable<object[]> NotAllowedPairs =>
    [
        [
            UserUtils.CreateUser(id: UserId.Create(2), roles : new HashSet < Role >() { Role.Customer }),
            UserUtils.CreateUser(id: UserId.Create(1), roles : new HashSet < Role >() { Role.Admin }),
        ],
        [
            UserUtils.CreateUser(id: UserId.Create(1), roles : new HashSet < Role >() { Role.Customer }),
            UserUtils.CreateUser(id: UserId.Create(4), roles : new HashSet < Role >() { Role.Customer }),
        ],
        [
            UserUtils.CreateUser(id: UserId.Create(1), roles : new HashSet < Role >() { Role.Admin }),
            UserUtils.CreateUser(id: UserId.Create(4), roles : new HashSet < Role >() { Role.Admin }),
        ]
    ];

    /// <summary>
    /// List of allowed pairs.
    /// </summary>
    public static IEnumerable<object[]> AllowedPairs =>
    [
        [
            UserUtils.CreateUser(id: UserId.Create(1) ,roles : new HashSet < Role >() { Role.Customer }),
            UserUtils.CreateUser(id: UserId.Create(1) ,roles : new HashSet < Role >() { Role.Customer })
        ],
        [
            UserUtils.CreateUser(id: UserId.Create(1) ,roles : new HashSet < Role >() { Role.Admin }),
            UserUtils.CreateUser(id: UserId.Create(2) ,roles : new HashSet < Role >() { Role.Customer })
        ],
        [
            UserUtils.CreateUser(id: UserId.Create(1) ,roles : new HashSet < Role >() { Role.Admin }),
            UserUtils.CreateUser(id: UserId.Create(1) ,roles : new HashSet < Role >() { Role.Admin })
        ],
    ];

    /// <summary>
    /// Tests that when the user to be updated exists and the current user has the right permissions the update is done successfully.
    /// </summary>
    [Theory]
    [MemberData(nameof(AllowedPairs))]
    public async Task HandleUpdateUser_WhenUserExists_UpdateTheUserCorrectly(
        User currentUser,
        User userToBeUpdated
    )
    {
        _mockUserRepository
            .SetupSequence(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(currentUser)
            .ReturnsAsync(userToBeUpdated);

        var command = UpdateUserCommandUtils.CreateCommand(name: "new name", phone: "19958274823");

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
            .SetupSequence(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(UserUtils.CreateUser())
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(UpdateUserCommandUtils.CreateCommand(), default))
            .Should()
            .ThrowAsync<UserNotFoundException>()
            .WithMessage("The user to be updated could not be found");
    }

    /// <summary>
    /// Tests that when trying to update a user's email to an already existing one,
    /// the handler throws a <see cref="UserAlreadyExistsException"/>.
    /// </summary>
    [Fact]
    public async Task HandleUpdateUser_WhenTryingToUpdateEmailWithExistingOne_ThrowsException()
    {
        var userToBeUpdatedNewEmail = EmailUtils.CreateEmail("existing_email@email.com");

        var userToBeUpdated = UserUtils.CreateUser();
        var conflictingUser = UserUtils.CreateUser(email: userToBeUpdatedNewEmail);
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
            .ThrowAsync<UserAlreadyExistsException>()
            .WithMessage("The email you entered is already in use");
    }

    /// <summary>
    /// Tests that a user cannot update another user if they do not have the right permissions.
    /// </summary>
    /// <param name="currentUser">The current user.</param>
    /// <param name="toBeUpdated">The user to be updated.</param>
    [Theory]
    [MemberData(nameof(NotAllowedPairs))]
    public async Task HandleUpdateUser_WhenUserTriesToUpdateSomeoneTheyAreNotAllowed_ThrowsException(
        User currentUser,
        User toBeUpdated
    )
    {
        _mockUserRepository
            .SetupSequence(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(currentUser)
            .ReturnsAsync(toBeUpdated);

        await FluentActions
            .Invoking(() => _handler.Handle(UpdateUserCommandUtils.CreateCommand(), default))
            .Should()
            .ThrowAsync<UserNotAllowedException>()
            .WithMessage($"The current user is not allowed to update the user with id {toBeUpdated.Id}");
    }
}
