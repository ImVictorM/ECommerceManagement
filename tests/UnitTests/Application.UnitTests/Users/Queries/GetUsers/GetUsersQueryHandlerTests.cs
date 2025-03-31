using Application.Common.Persistence.Repositories;
using Application.Users.Queries.GetUsers;
using Application.Users.DTOs.Filters;
using Application.UnitTests.Users.Queries.TestUtils;

using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Users.Queries.GetUsers;

/// <summary>
/// Unit tests for the <see cref="GetUsersQueryHandler"/> query handler.
/// </summary>
public class GetUsersQueryHandlerTests
{
    private readonly GetUsersQueryHandler _handler;
    private readonly Mock<IUserRepository> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetUsersQueryHandlerTests"/>
    /// class.
    /// </summary>
    public GetUsersQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        _handler = new GetUsersQueryHandler(
            _mockUserRepository.Object,
            Mock.Of<ILogger<GetUsersQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies the users are retrieved correctly.
    /// </summary>
    [Fact]
    public async Task HandleGetUsers_WithValidRequest_ReturnsUserResults()
    {
        var users = UserUtils.CreateCustomers(count: 5);

        _mockUserRepository
            .Setup(r => r.GetUsersAsync(
                It.IsAny<UserFilters>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(users);

        var query = GetUsersQueryUtils.CreateQuery();

        var response = await _handler.Handle(query, default);

        response.Select(u => u.Id)
            .Should().BeEquivalentTo(users.Select(u => u.Id.ToString()));

        _mockUserRepository.Verify(r =>
            r.GetUsersAsync(
                It.IsAny<UserFilters>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once()
        );
    }
}
