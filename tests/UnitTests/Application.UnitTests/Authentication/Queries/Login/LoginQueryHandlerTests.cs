using Application.Authentication.Common.Errors;
using Application.Authentication.Queries.Login;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Persistence;
using Application.UnitTests.Authentication.Queries.TestUtils;
using Application.UnitTests.TestUtils.Constants;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Models;

namespace Application.UnitTests.Authentication.Queries.Login;

/// <summary>
/// Tests for the login use case.
/// </summary>
public class LoginQueryHandlerTests
{
    /// <summary>
    /// The default message used when authentication errors occur.
    /// </summary>
    public const string LoginDefaultErrorMessage = "User email or password is incorrect";

    private readonly LoginQueryHandler _handler;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;

    /// <summary>
    /// List of valid login queries.
    /// </summary>
    /// <returns>A list of valid login queries.</returns>
    public static IEnumerable<object[]> ValidLoginQueries()
    {
        yield return new object[] { LoginQueryUtils.CreateQuery() };
        yield return new object[] { LoginQueryUtils.CreateQuery(email: "seven_777@email.com") };
        yield return new object[] { LoginQueryUtils.CreateQuery(password: "supersecret123") };
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginQueryHandlerTests"/> class.
    /// </summary>
    public LoginQueryHandlerTests()
    {
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();

        _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new LoginQueryHandler(
            _mockPasswordHasher.Object,
            _mockJwtTokenService.Object,
            _mockUnitOfWork.Object,
            new Mock<ILogger<LoginQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests if it is possible to authenticate an user with valid credentials.
    /// </summary>
    /// <param name="query">The login query.</param>
    /// <returns>An asynchronous testing operation.</returns>
    [Theory]
    [MemberData(nameof(ValidLoginQueries))]
    public async Task HandleLoginQuery_WhenCredentialsAreValid_ShouldAuthenticateAndReturnTokenAndUser(LoginQuery query)
    {
        _mockJwtTokenService.Setup(jwtTokenService => jwtTokenService.GenerateToken(It.IsAny<User>())).Returns(ApplicationConstants.Jwt.Token);
        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync(UserUtils.CreateUser(email: query.Email));
        _mockPasswordHasher.Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();

        result.User.Email.Value.Should().Be(query.Email);

        result.Token.Should().Be(ApplicationConstants.Jwt.Token);

        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Never);
    }

    /// <summary>
    /// Tests when the user is not found by email in the database.
    /// </summary>
    /// <returns>An asynchronous testing operation.</returns>
    [Fact]
    public async Task HandleLoginQuery_WhenEmailIsInvalid_ThrowsAnError()
    {
        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync((User?)null);

        var query = LoginQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(LoginDefaultErrorMessage);
    }

    /// <summary>
    /// Tests if it is not possible to authenticate an user with invalid password.
    /// </summary>
    /// <returns>An asynchronous testing operation.</returns>
    [Fact]
    public async Task HandleLoginQuery_WhenPasswordIsInvalid_ThrowsAnError()
    {
        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync(UserUtils.CreateUser());

        _mockPasswordHasher.Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var query = LoginQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(LoginDefaultErrorMessage);
    }

    /// <summary>
    /// Tests if it is not possible to authenticate an inactive user.
    /// </summary>
    /// <returns>An asynchronous testing operation.</returns>
    [Fact]
    public async Task HandleLoginQuery_WhenUserIsInactive_ThrowsAnError()
    {
        var mockUser = UserUtils.CreateUser();

        mockUser.Deactivate();

        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync(mockUser);

        var query = LoginQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(LoginDefaultErrorMessage);
    }
}
