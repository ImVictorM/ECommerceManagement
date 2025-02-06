using Application.Authentication.Commands.RegisterCustomer;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Authentication;
using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Common.Errors;
using Application.UnitTests.TestUtils.Behaviors;
using Application.UnitTests.Authentication.Commands.TestUtils;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;

using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Authentication.Commands.RegisterCustomer;

/// <summary>
/// Unit tests for the <see cref="RegisterCustomerCommandHandler"/> class.
/// </summary>
public class RegisterCustomerCommandHandlerTests
{
    private readonly RegisterCustomerCommandHandler _handler;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="RegisterCustomerCommandHandlerTests"/> class.
    /// </summary>
    public RegisterCustomerCommandHandlerTests()
    {
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();

        _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);

        _handler = new RegisterCustomerCommandHandler(
            _mockJwtTokenService.Object,
            _mockPasswordHasher.Object,
            _mockUnitOfWork.Object,
            new Mock<ILogger<RegisterCustomerCommandHandler>>().Object
        );
    }

    /// <summary>
    /// List containing valid register commands.
    /// </summary>
    public static IEnumerable<object[]> ValidRegisterCommands =>
    [
        [RegisterCustomerCommandUtils.CreateCommand()],
        [RegisterCustomerCommandUtils.CreateCommand(name: "Apparently not jack")],
        [RegisterCustomerCommandUtils.CreateCommand(password: "super-secret123")],
        [RegisterCustomerCommandUtils.CreateCommand(email: "my_email@email.com")],
    ];

    /// <summary>
    /// Tests if a user is correctly created when the user is valid.
    /// </summary>
    /// <param name="registerCommand">The register command.</param>
    [Theory]
    [MemberData(nameof(ValidRegisterCommands))]
    public async Task HandleRegisterCustomerCommand_WhenUserIsValid_ShouldCreateAndReturnUserPlusAuthenticationToken(RegisterCustomerCommand registerCommand)
    {
        var generatedToken = "generated-token";
        var createdUserId = UserId.Create(2);

        _mockUserRepository
            .Setup(r => r.FindOneOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User?)null);

        _mockJwtTokenService
            .Setup(r => r.GenerateToken(It.IsAny<IdentityUser>()))
            .Returns(generatedToken);

        _mockPasswordHasher
            .Setup(r => r.Hash(It.IsAny<string>()))
            .Returns(PasswordHashUtils.Create());

        MockEFCoreBehaviors.MockSetEntityIdBehavior(_mockUserRepository, _mockUnitOfWork, createdUserId);

        var result = await _handler.Handle(registerCommand, default);

        result.Should().NotBeNull();
        result.User.Id.Should().Be(createdUserId);
        result.User.Name.Should().Be(registerCommand.Name);
        result.User.Email.Value.Should().Be(registerCommand.Email);
        result.User.UserRoles.Count.Should().Be(1);
        result.User.UserRoles.First().RoleId.Should().Be(Role.Customer.Id);
        result.User.UserAddresses.Count.Should().Be(0);
        result.Token.Should().Be(generatedToken);
        result.User.IsActive.Should().BeTrue();

        _mockPasswordHasher.Verify(m => m.Hash(registerCommand.Password), Times.Once);
        _mockJwtTokenService.Verify(m => m.GenerateToken(It.IsAny<IdentityUser>()), Times.Once);
        _mockUserRepository.Verify(m => m.AddAsync(result.User), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Tests if it throws an error when a user with the same email already exists.
    /// </summary>
    [Fact]
    public async Task HandleRegisterCustomerCommand_WhenUserWithEmailAlreadyExists_ThrowsError()
    {
        var testEmail = EmailUtils.CreateEmail();

        _mockUserRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryUserByEmailSpecification>()))
            .ReturnsAsync(UserUtils.CreateUser(email: testEmail));

        var registerCommand = RegisterCustomerCommandUtils.CreateCommand(email: testEmail.ToString());

        await FluentActions.Invoking(() => _handler.Handle(registerCommand, default))
           .Should()
           .ThrowAsync<EmailConflictException>();
    }
}
