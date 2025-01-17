using Application.Common.Errors;
using Application.Common.Persistence;
using Application.Common.Security.Authorization.Roles;
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
    /// Not allowed pairs.
    /// </summary>
    public static IEnumerable<object[]> NotAllowedPairs =>
    [
        [
            UserUtils.CreateUser(id: UserId.Create(1), roles: new HashSet<Role>() { Role.Customer }),
            UserUtils.CreateUser(id: UserId.Create(2), roles: new HashSet<Role>() { Role.Customer })
        ],
        [
            UserUtils.CreateUser(id: UserId.Create(1), roles: new HashSet<Role>() { Role.Customer }),
            UserUtils.CreateUser(id: UserId.Create(2), roles: new HashSet<Role>() { Role.Admin })
        ],
        [
            UserUtils.CreateUser(id: UserId.Create(1), roles : new HashSet < Role >() { Role.Admin }),
            UserUtils.CreateUser(id: UserId.Create(2), roles : new HashSet < Role >() { Role.Admin })
        ],
        [
            UserUtils.CreateUser(id: UserId.Create(1), roles : new HashSet < Role >() { Role.Admin }),
            UserUtils.CreateUser(id: UserId.Create(1), roles : new HashSet < Role >() { Role.Admin })
        ]
    ];

    /// <summary>
    /// List of allowed pairs.
    /// </summary>
    public static IEnumerable<object[]> AllowedPairs =>
    [
        [
            UserUtils.CreateUser(id: UserId.Create(1), active: true, roles: new HashSet<Role>() { Role.Customer }),
            UserUtils.CreateUser(id: UserId.Create(1), active: true, roles : new HashSet<Role>() { Role.Customer })
        ],
        [
            UserUtils.CreateUser(id: UserId.Create(1), roles : new HashSet < Role >() { Role.Admin }),
            UserUtils.CreateUser(id: UserId.Create(2), active: true, roles: new HashSet<Role>() { Role.Customer })
        ]
    ];

    /// <summary>
    /// Tests that when the user to be deactivated exists and the current user has the right permissions the deactivation occurs.
    /// </summary>
    [Theory]
    [MemberData(nameof(AllowedPairs))]
    public async Task HandleDeactivateUser_WhenUserExists_DeactivateItAndSave(
        User currentUser,
        User userToBeDeactivate
    )
    {
        _mockUserRepository
            .SetupSequence(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(currentUser)
            .ReturnsAsync(userToBeDeactivate);

        await _handler.Handle(DeactivateUserCommandUtils.CreateCommand(), default);

        userToBeDeactivate.IsActive.Should().BeFalse();
        _mockUnitOfWork.Verify(uof => uof.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Tests when a user to be deactivated does not exist the command handler completes without throwing any exceptions.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateUser_WhenUserDoesNotExist_ReturnsWithoutThrowing()
    {
        var currentUser = UserUtils.CreateUser(roles: new HashSet<Role>() { Role.Admin }, id: UserId.Create(1));

        _mockUserRepository
            .SetupSequence(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(currentUser)
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(DeactivateUserCommandUtils.CreateCommand(), default))
            .Should()
            .NotThrowAsync();

        _mockUnitOfWork.Verify(uof => uof.SaveChangesAsync(), Times.Never());
    }

    /// <summary>
    /// Tests a user without the right permissions cannot use the deactivation feature.
    /// </summary>
    /// <param name="currentUser">The current user.</param>
    /// <param name="userToDeactivate">The user to be deactivated.</param>
    [Theory]
    [MemberData(nameof(NotAllowedPairs))]
    public async Task HandleDeactivateUser_WhenCurrentUserDoesNotHaveTheRightPermission_ThrowsException(
        User currentUser,
        User userToDeactivate
    )
    {
        _mockUserRepository
            .SetupSequence(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveUserByIdSpecification>()))
            .ReturnsAsync(currentUser)
            .ReturnsAsync(userToDeactivate);

        await FluentActions
            .Invoking(() => _handler.Handle(DeactivateUserCommandUtils.CreateCommand(), default))
            .Should()
            .ThrowAsync<UserNotAllowedException>();
    }
}
