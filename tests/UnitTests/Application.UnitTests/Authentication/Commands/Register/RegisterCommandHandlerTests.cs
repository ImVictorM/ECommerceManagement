using System.Linq.Expressions;
using Application.Authentication.Commands.Register;
using Application.Common.Errors;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Persistence;
using Application.UnitTests.Authentication.Commands.TestUtils;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Authentication.Commands.Register;

/// <summary>
/// Tests for the registration use case.
/// </summary>
public class RegisterCommandHandlerTests
{
    private readonly RegisterCommandHandler _handler;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="RegisterCommandHandlerTests"/> class.
    /// </summary>
    public RegisterCommandHandlerTests()
    {
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();

        _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new RegisterCommandHandler(
            _mockJwtTokenService.Object,
            _mockPasswordHasher.Object,
            _mockUnitOfWork.Object,
            new Mock<ILogger<RegisterCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests if a user is correctly created when the user is valid.
    /// </summary>
    /// <param name="registerCommand">The register command.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(ValidRegisterCommands))]
    public async Task HandleRegisterCommand_WhenUserIsValid_ShouldCreateAndReturnUserPlusAuthenticationToken(RegisterCommand registerCommand)
    {
        var expectedToken = "jwt-token";

        _mockUserRepository
            .Setup(r => r.FindOneOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);

        _mockJwtTokenService
            .Setup(r => r.GenerateToken(It.IsAny<User>()))
            .Returns(expectedToken);

        _mockPasswordHasher
            .Setup(r => r.Hash(It.IsAny<string>()))
            .Returns((DomainConstants.User.PasswordHash, DomainConstants.User.PasswordSalt));

        var result = await _handler.Handle(registerCommand, default);

        result.Should().NotBeNull();
        result.User.Name.Should().Be(registerCommand.Name);
        result.User.Email.Value.Should().Be(registerCommand.Email);
        result.User.UserRoles.Count.Should().Be(1);
        result.User.UserAddresses.Count.Should().Be(0);
        result.Token.Should().Be(expectedToken);

        _mockPasswordHasher.Verify(m => m.Hash(registerCommand.Password), Times.Once);
        _mockJwtTokenService.Verify(m => m.GenerateToken(result.User), Times.Once);
        _mockUserRepository.Verify(m => m.AddAsync(result.User), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Tests if it throws an error when a user with the same email already exists.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task HandleRegisterCommand_WhenUserWithEmailAlreadyExists_ThrowsAnError()
    {
        var testEmail = "test@email.com";

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryUserByEmailSpecification>()))
            .ReturnsAsync(UserUtils.CreateUser(email: testEmail));

        var registerCommand = RegisterCommandUtils.CreateCommand(email: testEmail);

        await FluentActions.Invoking(() => _handler.Handle(registerCommand, default))
           .Should()
           .ThrowAsync<UserAlreadyExistsException>()
           .WithMessage("The user already exists");
    }

    /// <summary>
    /// List containing valid register commands.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> ValidRegisterCommands()
    {
        yield return new object[] { RegisterCommandUtils.CreateCommand() };
        yield return new object[] { RegisterCommandUtils.CreateCommand(name: "Apparently not jack") };
        yield return new object[] { RegisterCommandUtils.CreateCommand(password: "super-secret123") };
        yield return new object[] { RegisterCommandUtils.CreateCommand(email: "myemail@email.com") };
    }
}
