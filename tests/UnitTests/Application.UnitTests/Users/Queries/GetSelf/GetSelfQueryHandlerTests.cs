using Application.Common.Persistence.Repositories;
using Application.Common.Security.Identity;
using Application.Users.Queries.GetSelf;
using Application.Users.Errors;

using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UnitTests.TestUtils;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
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
    private readonly Mock<IUserRepository> _mockUserRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSelfQueryHandlerTests"/>
    /// class.
    /// </summary>
    public GetSelfQueryHandlerTests()
    {
        _mockIdentityProvider = new Mock<IIdentityProvider>();
        _mockUserRepository = new Mock<IUserRepository>();

        _handler = new GetSelfQueryHandler(
            _mockIdentityProvider.Object,
            _mockUserRepository.Object,
            Mock.Of<ILogger<GetSelfQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies that the handler retrieves and returns the current user's
    /// information.
    /// </summary>
    [Fact]
    public async Task HandleGetSelfQuery_WhenUserExists_ReturnsUserResult()
    {
        var currentUserIdentity = new IdentityUser("1", [Role.Customer]);
        var currentUserId = UserId.Create(currentUserIdentity.Id);

        var user = UserUtils.CreateCustomer(
            id: currentUserId
        );

        _mockIdentityProvider
            .Setup(provider => provider.GetCurrentUserIdentity())
            .Returns(currentUserIdentity);

        _mockUserRepository
            .Setup(repo => repo.FindByIdAsync(
                currentUserId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(user);

        var query = new GetSelfQuery();

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();
        result.Id.Should().Be(currentUserId.ToString());
        result.Email.Should().Be(user.Email.ToString());
    }

    /// <summary>
    /// Verifies that the handler throws a <see cref="UserNotFoundException"/>
    /// when the user does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetSelfQuery_WhenUserDoesNotExist_ThrowsException()
    {
        var currentUserIdentity = new IdentityUser("1", [Role.Customer]);
        var currentUserId = UserId.Create(currentUserIdentity.Id);

        _mockIdentityProvider
            .Setup(provider => provider.GetCurrentUserIdentity())
            .Returns(currentUserIdentity);

        _mockUserRepository
            .Setup(repo => repo.FindByIdAsync(
                currentUserId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((User?)null);

        var query = new GetSelfQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<UserNotFoundException>();
    }
}
