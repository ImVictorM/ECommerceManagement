using Domain.UnitTests.TestUtils;
using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using Application.Common.Persistence;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;
using Application.Authentication.Errors;
using Application.UnitTests.Authentication.Queries.TestUtils;
using Application.Authentication.Queries.LoginCarrier;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Authentication.Queries.LoginCarrier;

/// <summary>
/// Unit tests for the <see cref="LoginCarrierQueryHandler"/> handler.
/// </summary>
public class LoginCarrierQueryHandlerTests
{
    private readonly LoginCarrierQueryHandler _handler;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Carrier, CarrierId>> _mockCarrierRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierQueryHandlerTests"/> class.
    /// </summary>
    public LoginCarrierQueryHandlerTests()
    {
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCarrierRepository = new Mock<IRepository<Carrier, CarrierId>>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();

        _mockUnitOfWork.Setup(u => u.CarrierRepository).Returns(_mockCarrierRepository.Object);

        _handler = new LoginCarrierQueryHandler(
            _mockUnitOfWork.Object,
            _mockPasswordHasher.Object,
            _mockJwtTokenService.Object,
            new Mock<ILogger<LoginCarrierQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the carrier is successfully authenticated when the carrier exists and the credentials are valid.
    /// </summary>
    [Fact]
    public async Task HandleLoginCarrierQuery_WhenCredentialsAreValid_ReturnsTokenAndCarrierData()
    {
        var generatedToken = "generated-token";

        var query = LoginCarrierQueryUtils.CreateQuery();

        var carrier = CarrierUtils.CreateCarrier(
            id: CarrierId.Create(1),
            email: EmailUtils.CreateEmail(query.Email)
        );

        _mockCarrierRepository
            .Setup(repository => repository.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Carrier, bool>>>()))
            .ReturnsAsync(carrier);

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

        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(), Times.Never());
    }

    /// <summary>
    /// Verifies and exception is thrown when the carrier email is incorrect.
    /// </summary>
    [Fact]
    public async Task HandleLoginCarrierQuery_WhenEmailIsIncorrect_ThrowsError()
    {
        _mockCarrierRepository
            .Setup(repository => repository.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Carrier, bool>>>()))
            .ReturnsAsync((Carrier?)null);

        var query = LoginCarrierQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }

    /// <summary>
    /// Verifies and exception is thrown when the carrier email is correct and the password is incorrect.
    /// </summary>
    [Fact]
    public async Task HandleLoginCarrierQuery_WhenEmailIsCorrectAndPasswordIsIncorrect_ThrowsError()
    {
        _mockCarrierRepository
            .Setup(repository => repository.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Carrier, bool>>>()))
            .ReturnsAsync(CarrierUtils.CreateCarrier());

        _mockPasswordHasher
            .Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<PasswordHash>()))
            .Returns(false);

        var query = LoginCarrierQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }
}
