using Application.Authentication.Common.Errors;
using Application.Authentication.Queries.Login;
using Application.UnitTests.Authentication.Queries.TestUtils;
using Application.Common.Security.Authentication;
using Application.Common.Persistence;
using Application.Common.Security.Identity;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Models;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;
using SharedKernel.Interfaces;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Authentication.Queries.Login;

/// <summary>
/// Unit tests for the <see cref="LoginQueryHandler"/> class.
/// </summary>
public class LoginQueryHandlerTests
{
    private const string LoginDefaultErrorMessage = "User email or password is incorrect";

    private readonly LoginQueryHandler _handler;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;

    /// <summary>
    /// List of valid login queries.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidLoginQueries =
    [
        [LoginQueryUtils.CreateQuery()],
        [LoginQueryUtils.CreateQuery(email: "seven_777@email.com")],
        [LoginQueryUtils.CreateQuery(password: "supersecret123")]
    ];

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
    [Theory]
    [MemberData(nameof(ValidLoginQueries))]
    public async Task HandleLoginQuery_WhenCredentialsAreValid_ReturnsTokenAndUser(LoginQuery query)
    {
        var generatedToken = "generated-token";

        var user = UserUtils.CreateUser(
            id: UserId.Create(1),
            email: EmailUtils.CreateEmail(query.Email)
        );

        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<ISpecificationQuery<User>>()))
            .ReturnsAsync(user);

        _mockJwtTokenService
            .Setup(jwtTokenService => jwtTokenService.GenerateToken(It.IsAny<IdentityUser>()))
            .Returns(generatedToken);

        _mockPasswordHasher
            .Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<PasswordHash>()))
            .Returns(true);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();

        result.User.Email.ToString().Should().Be(query.Email);

        result.Token.Should().Be(generatedToken);

        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Never);
    }

    /// <summary>
    /// Tests when the user is not found by email in the database.
    /// </summary>
    [Fact]
    public async Task HandleLoginQuery_WhenEmailIsIncorrect_ThrowsError()
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
    [Fact]
    public async Task HandleLoginQuery_WhenEmailIsCorrectAndPasswordIsIncorrect_ThrowsError()
    {
        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync(UserUtils.CreateUser());

        _mockPasswordHasher
            .Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<PasswordHash>()))
            .Returns(false);

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
    [Fact]
    public async Task HandleLoginQuery_WhenUserIsInactive_ThrowsError()
    {
        var userInactive = UserUtils.CreateUser(active: false);

        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync(userInactive);

        var query = LoginQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(LoginDefaultErrorMessage);
    }
}
