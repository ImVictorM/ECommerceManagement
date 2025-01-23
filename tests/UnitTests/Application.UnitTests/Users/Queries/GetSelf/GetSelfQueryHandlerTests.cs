using Application.Common.Errors;
using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Users.Queries.GetSelf;
using Application.Common.Security.Authorization.Roles;

using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Users.Queries.GetSelf;

/// <summary>
/// Unit tests for the <see cref="GetSelfQueryHandler"/> handler.
/// </summary>
public class GetSelfQueryHandlerTests
{
    private readonly GetSelfQueryHandler _handler;
    private readonly Mock<IIdentityProvider> _mockIdentityProvider;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSelfQueryHandlerTests"/> class.
    /// </summary>
    public GetSelfQueryHandlerTests()
    {
        _mockIdentityProvider = new Mock<IIdentityProvider>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();

        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new GetSelfQueryHandler(_mockIdentityProvider.Object, _mockUnitOfWork.Object);
    }

    /// <summary>
    /// Verifies that the handler retrieves and returns the current user's information.
    /// </summary>
    [Fact]
    public async Task HandleGetSelfQuery_WhenUserExists_ReturnsUserResult()
    {
        var currentUserIdentity = new IdentityUser("1", [Role.Customer.Name]);
        var currentUserId = UserId.Create(currentUserIdentity.Id);

        var user = UserUtils.CreateUser(
            id: UserId.Create(currentUserIdentity.Id),
            roles: new HashSet<UserRole>()
            {
                UserRole.Create(Role.Customer.Id)
            }
        );

        _mockIdentityProvider
            .Setup(provider => provider.GetCurrentUserIdentity())
            .Returns(currentUserIdentity);
        _mockUserRepository
            .Setup(repo => repo.FindByIdAsync(currentUserId))
            .ReturnsAsync(user);

        var query = new GetSelfQuery();

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.User.Should().Be(user);
    }

    /// <summary>
    /// Verifies that the handler throws a <see cref="UserNotFoundException"/> when the user does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetSelfQuery_WhenUserDoesNotExist_ThrowsException()
    {
        var currentUserIdentity = new IdentityUser("1", [Role.Customer.Name]);
        var currentUserId = UserId.Create(currentUserIdentity.Id);

        _mockIdentityProvider
            .Setup(provider => provider.GetCurrentUserIdentity())
            .Returns(currentUserIdentity);

        _mockUserRepository
            .Setup(repo => repo.FindByIdAsync(currentUserId))
            .ReturnsAsync((User?)null);

        var query = new GetSelfQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<UserNotFoundException>();
    }
}
