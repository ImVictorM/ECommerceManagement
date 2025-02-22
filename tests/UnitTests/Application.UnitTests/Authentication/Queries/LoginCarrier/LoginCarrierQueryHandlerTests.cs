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
    private readonly Mock<ICarrierRepository> _mockCarrierRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierQueryHandlerTests"/> class.
    /// </summary>
    public LoginCarrierQueryHandlerTests()
    {
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _mockCarrierRepository = new Mock<ICarrierRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();

        _handler = new LoginCarrierQueryHandler(
            _mockCarrierRepository.Object,
            _mockPasswordHasher.Object,
            _mockJwtTokenService.Object,
            new Mock<ILogger<LoginCarrierQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the carrier is successfully authenticated when the carrier exists and the credentials are valid.
    /// </summary>
    [Fact]
    public async Task HandleLoginCarrierQuery_WithValidCredentials_ReturnsCarrierWithAuthenticationToken()
    {
        var generatedToken = "generated-token";

        var query = LoginCarrierQueryUtils.CreateQuery();

        var carrier = CarrierUtils.CreateCarrier(
            id: CarrierId.Create(1),
            email: EmailUtils.CreateEmail(query.Email)
        );

        _mockCarrierRepository
            .Setup(repository => repository.FindByEmail(
                carrier.Email,
                It.IsAny<CancellationToken>()
            ))
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
    }

    /// <summary>
    /// Verifies and exception is thrown when the carrier email is incorrect.
    /// </summary>
    [Fact]
    public async Task HandleLoginCarrierQuery_WithIncorrectEmail_ThrowsError()
    {
        var query = LoginCarrierQueryUtils.CreateQuery();

        _mockCarrierRepository
            .Setup(repository => repository.FindByEmail(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Carrier?)null);

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
    public async Task HandleLoginCarrierQuery_WithCorrectEmailAndWrongPassword_ThrowsError()
    {
        var query = LoginCarrierQueryUtils.CreateQuery();

        _mockCarrierRepository
            .Setup(repository => repository.FindByEmail(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(CarrierUtils.CreateCarrier());

        _mockPasswordHasher
            .Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<PasswordHash>()))
            .Returns(false);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<AuthenticationFailedException>()
            .WithMessage(AuthenticationErrorMessages.AuthenticationFailed);
    }
}
