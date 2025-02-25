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
    /// Initiates a new instance of the <see cref="GetUserByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetUserByIdQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        _handler = new GetUserByIdQueryHandler(
            _mockUserRepository.Object,
            new Mock<ILogger<GetUserByIdQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests if the user is returned when they exists.
    /// </summary>
    [Fact]
    public async Task HandleGetUserById_WhenUserExists_ReturnsUser()
    {
        var mockUser = UserUtils.CreateCustomer();

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<UserId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(mockUser);

        var result = await _handler.Handle(GetUserByIdQueryUtils.CreateQuery(), default);

        result.User.Should().NotBeNull();
        result.User.Should().BeEquivalentTo(mockUser);
    }

    /// <summary>
    /// Tests if an error is thrown when the user is not found.
    /// </summary>
    [Fact]
    public async Task HandleGetUserById_WhenUserDoesNotExist_ThrowsError()
    {
        _mockUserRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<UserId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((User?)null);

        var idToBeFound = "1";

        await FluentActions
            .Invoking(() => _handler.Handle(GetUserByIdQueryUtils.CreateQuery(idToBeFound), default))
            .Should()
            .ThrowAsync<UserNotFoundException>()
            .WithMessage($"User with id {idToBeFound} was not found");
    }
}
