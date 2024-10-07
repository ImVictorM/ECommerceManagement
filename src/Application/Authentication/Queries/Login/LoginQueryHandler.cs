using Application.Authentication.Common;
using Application.Common.Errors;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Persistence;
using Domain.Common.ValueObjects;
using Domain.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Query handler for the <see cref="LoginQuery"/> query.
/// Handles user authentication.
/// </summary>
public partial class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    /// <summary>
    /// Service to hash and verify passwords.
    /// </summary>
    private readonly IPasswordHasher _passwordHasher;
    /// <summary>
    /// Service to generate authentication tokens.
    /// </summary>
    private readonly IJwtTokenService _jwtTokenGenerator;
    /// <summary>
    /// Component to interact with the repositories and persist changes.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginQueryHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">Token service.</param>
    /// <param name="passwordHasher">Password hash service.</param>
    /// <param name="unitOfWork">The unity of work.</param>
    /// <param name="logger">The query handler logger.</param>
    public LoginQueryHandler(
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenGenerator,
        IUnitOfWork unitOfWork,
        ILogger<LoginQueryHandler> logger
    )
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Handle user authentication.
    /// </summary>
    /// <param name="query">The query that triggers the authentication process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user with authentication token.</returns>
    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var defaultUserNotFoundErrorMessage = "User email or password is incorrect.";
        var inputEmail = Email.Create(query.Email);

        LogHandlingLoginQuery(inputEmail.Value);

        User? user = await _unitOfWork.UserRepository.FindOneOrDefaultAsync(user => user.Email == inputEmail);

        if (user == null)
        {
            LogUserNotFound(query.Email);
            throw new BadRequestException(defaultUserNotFoundErrorMessage);
        }

        if (!_passwordHasher.Verify(query.Password, user.PasswordHash.GetPasswordHash(), user.PasswordHash.GetPasswordSalt()))
        {
            LogInvalidPassword(query.Email);
            throw new BadRequestException(defaultUserNotFoundErrorMessage);
        }

        LogSuccessfullyAuthenticatedUser(query.Email);

        var token = await _jwtTokenGenerator.GenerateToken(user);
        LogTokenGenerated(user.Id.Value);

        return new AuthenticationResult(user, token);
    }
}
