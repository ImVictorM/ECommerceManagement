using Application.Authentication.Errors;
using Application.Authentication.Queries.LoginUser;
using Application.UnitTests.Authentication.Queries.TestUtils;
using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

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
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;

    /// <summary>
    /// Provides a list of valid login queries.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidLoginQueries =
    [
        [LoginUserQueryUtils.CreateQuery()],
        [LoginUserQueryUtils.CreateQuery(email: "seven_777@email.com")],
        [LoginUserQueryUtils.CreateQuery(password: "supersecret123")]
    ];

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginUserQueryHandlerTests"/>
    /// class.
    /// </summary>
    public LoginUserQueryHandlerTests()
    {
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();

        _handler = new LoginUserQueryHandler(
            _mockPasswordHasher.Object,
            _mockJwtTokenService.Object,
            _mockUserRepository.Object,
            new Mock<ILogger<LoginUserQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies it is possible to authenticate a user with valid credentials.
    /// </summary>
    /// <param name="query">The login query.</param>
    [Theory]
    [MemberData(nameof(ValidLoginQueries))]
    public async Task HandleLoginUserQuery_WithValidCredentials_ReturnsAuthenticationResult(
        LoginUserQuery query
    )
    {
        var generatedToken = "generated-token";

        var user = UserUtils.CreateCustomer(
            id: UserId.Create(1),
            email: EmailUtils.CreateEmail(query.Email)
        );

        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(
                It.IsAny<ISpecificationQuery<User>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(user);

        _mockJwtTokenService
            .Setup(jwtTokenService => jwtTokenService.GenerateToken(
                It.IsAny<IdentityUser>()
            ))
            .Returns(generatedToken);

        _mockPasswordHasher
            .Setup(hasher => hasher.Verify(
                It.IsAny<string>(),
                It.IsAny<PasswordHash>()
            ))
            .Returns(true);

        var result = await _handler.Handle(query, default);

        result.Should().NotBeNull();

        result.User.Email.Should().Be(query.Email);

        result.Token.Should().Be(generatedToken);
    }

    /// <summary>
    /// Verifies an exception is thrown when the email is incorrect.
    /// </summary>
    [Fact]
    public async Task HandleLoginUserQuery_WithIncorrectEmail_ThrowsError()
    {
        var query = LoginUserQueryUtils.CreateQuery();

        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(
                It.IsAny<ISpecificationQuery<User>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }

    /// <summary>
    /// Verifies an exception is thrown when the password is incorrect.
    /// </summary>
    [Fact]
    public async Task HandleLoginUserQuery_WithWrongPassword_ThrowsError()
    {
        var query = LoginUserQueryUtils.CreateQuery();

        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(
                It.IsAny<ISpecificationQuery<User>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(UserUtils.CreateCustomer());

        _mockPasswordHasher
            .Setup(hasher => hasher.Verify(
                It.IsAny<string>(),
                It.IsAny<PasswordHash>()
            ))
            .Returns(false);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }

    /// <summary>
    /// Verifies an exception is thrown when the user is inactive.
    /// </summary>
    [Fact]
    public async Task HandleLoginUserQuery_WhenUserIsInactive_ThrowsError()
    {
        var userInactive = UserUtils.CreateCustomer(active: false);

        var query = LoginUserQueryUtils.CreateQuery();

        _mockUserRepository
            .Setup(repository => repository.FindFirstSatisfyingAsync(
                It.IsAny<ISpecificationQuery<User>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(userInactive);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }
}
