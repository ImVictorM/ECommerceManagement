using Application.Common.Persistence.Repositories;
using Application.UnitTests.Users.Queries.TestUtils;
using Application.Users.Errors;
using Application.Users.Queries.GetUserById;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Users.Queries.GetUserById;

/// <summary>
/// Unit tests for the <see cref="GetUserByIdQueryHandler"/> query handler.
/// </summary>
public class GetUserByIdQueryHandlerTests
{
    private readonly GetUserByIdQueryHandler _handler;
    private readonly Mock<IUserRepository> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="GetUserByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetUserByIdQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        _handler = new GetUserByIdQueryHandler(
            _mockUserRepository.Object,
            Mock.Of<ILogger<GetUserByIdQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies the user is retrieved when the user exists.
    /// </summary>
    [Fact]
    public async Task HandleGetUserByIdQuery_WhenUserExists_ReturnsUser()
    {
        var user = UserUtils.CreateCustomer(id: UserId.Create(1));

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<UserId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(user);

        var result = await _handler.Handle(
            GetUserByIdQueryUtils.CreateQuery(),
            default
        );

        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id.ToString());
        result.Email.Should().Be(user.Email.ToString());
    }

    /// <summary>
    /// Verifies an exception is thrown when the user does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetUserByIdQuery_WhenUserDoesNotExist_ThrowsError()
    {
        var userId = "1";

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<UserId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(
                GetUserByIdQueryUtils.CreateQuery(userId),
                default
            ))
            .Should()
            .ThrowAsync<UserNotFoundException>();
    }
}
