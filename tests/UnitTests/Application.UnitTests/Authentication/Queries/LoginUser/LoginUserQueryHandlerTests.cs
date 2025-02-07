using Application.Authentication.Errors;
using Application.Authentication.Queries.LoginUser;
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

namespace Application.UnitTests.Authentication.Queries.LoginUser;

/// <summary>
/// Unit tests for the <see cref="LoginUserQueryHandler"/> class.
/// </summary>
public class LoginUserQueryHandlerTests
{
    private readonly LoginUserQueryHandler _handler;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;

    /// <summary>
    /// List of valid login queries.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidLoginQueries =
    [
        [LoginUserQueryUtils.CreateQuery()],
        [LoginUserQueryUtils.CreateQuery(email: "seven_777@email.com")],
        [LoginUserQueryUtils.CreateQuery(password: "supersecret123")]
    ];

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginUserQueryHandlerTests"/> class.
    /// </summary>
    public LoginUserQueryHandlerTests()
    {
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();

        _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new LoginUserQueryHandler(
            _mockPasswordHasher.Object,
            _mockJwtTokenService.Object,
            _mockUnitOfWork.Object,
            new Mock<ILogger<LoginUserQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests if it is possible to authenticate an user with valid credentials.
    /// </summary>
    /// <param name="query">The login query.</param>
    [Theory]
    [MemberData(nameof(ValidLoginQueries))]
    public async Task HandleLoginUserQuery_WhenCredentialsAreValid_ReturnsTokenAndUser(LoginUserQuery query)
    {
        var generatedToken = "generated-token";

        var user = UserUtils.CreateCustomer(
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

        result.AuthenticatedIdentity.Email.ToString().Should().Be(query.Email);

        result.Token.Should().Be(generatedToken);

        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Never);
    }

    /// <summary>
    /// Tests when the user is not found by email in the database.
    /// </summary>
    [Fact]
    public async Task HandleLoginUserQuery_WhenEmailIsIncorrect_ThrowsError()
    {
        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync((User?)null);

        var query = LoginUserQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }

    /// <summary>
    /// Tests if it is not possible to authenticate an user with invalid password.
    /// </summary>
    [Fact]
    public async Task HandleLoginUserQuery_WhenEmailIsCorrectAndPasswordIsIncorrect_ThrowsError()
    {
        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync(UserUtils.CreateCustomer());

        _mockPasswordHasher
            .Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<PasswordHash>()))
            .Returns(false);

        var query = LoginUserQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }

    /// <summary>
    /// Tests if it is not possible to authenticate an inactive user.
    /// </summary>
    [Fact]
    public async Task HandleLoginUserQuery_WhenUserIsInactive_ThrowsError()
    {
        var userInactive = UserUtils.CreateCustomer(active: false);

        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(It.IsAny<CompositeQuerySpecification<User>>()))
            .ReturnsAsync(userInactive);

        var query = LoginUserQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }
}
