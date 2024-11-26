using System.Linq.Expressions;
using Application.Common.Interfaces.Persistence;
using Application.UnitTests.Users.Queries.TestUtils;
using Application.Users.Queries.GetAllUsers;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Users.Queries.GetAllUsers;

/// <summary>
/// Test the behaviors of the <see cref="GetAllUsersQueryHandler"/> query handler.
/// </summary>
public class GetAllUsersQueryHandlerTests
{
    private readonly GetAllUsersQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllUsersQueryHandlerTests"/> class.
    /// </summary>
    public GetAllUsersQueryHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();

        _mockUnitOfWork.Setup(uw => uw.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new GetAllUsersQueryHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<GetAllUsersQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests if it queries the users with the active filter correctly.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task HandleGetAllUsers_WhenFilteringForUsersThatIsActive_QueryOnlyActiveUsers()
    {
        var mockActiveUsers = UserUtils.CreateUsers(count: 5, active: true);

        _mockUserRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(mockActiveUsers);

        var query = GetAllUsersQueryUtils.CreateQuery(isActive: true);

        var response = await _handler.Handle(query, default);

        response.Users.Should().BeEquivalentTo(mockActiveUsers);
        _mockUserRepository.Verify(r => r.FindAllAsync(user => user.IsActive == query.IsActive), Times.Once());
    }

    /// <summary>
    /// Tests if it queries the users with the inactive filter correctly.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task HandleGetAllUsers_WhenFilteringForUsersThatIsInactive_QueryOnlyInactiveUsers()
    {
        var mockInactiveUsers = UserUtils.CreateUsers(count: 5, active: false);

        _mockUserRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(mockInactiveUsers);

        var query = GetAllUsersQueryUtils.CreateQuery(isActive: false);

        var response = await _handler.Handle(query, default);

        response.Users.Should().BeEquivalentTo(mockInactiveUsers);
        _mockUserRepository.Verify(r => r.FindAllAsync(user => user.IsActive == query.IsActive), Times.Once());
    }

    /// <summary>
    /// Tests if it queries the users without any filters correctly.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task HandleGetAllUsers_WhenGettingAllUsersWithoutFilter_QueryAllTheUsers()
    {
        var mockAllUsers = UserUtils.CreateUsers(count: 5);

        _mockUserRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(mockAllUsers);

        var query = GetAllUsersQueryUtils.CreateQuery();

        var response = await _handler.Handle(query, default);

        response.Users.Should().BeEquivalentTo(mockAllUsers);
        _mockUserRepository.Verify(r => r.FindAllAsync(null), Times.Once());
    }
}
