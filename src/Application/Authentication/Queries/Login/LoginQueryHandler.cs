using Application.Authentication.Common;
using Application.Common.Errors;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate;
using MediatR;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Query handler for the <see cref="LoginQuery"/> query.
/// Handles user authentication.
/// </summary>
public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
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
    public LoginQueryHandler(
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenGenerator,
        IUnitOfWork unitOfWork
    )
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
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

        User user = await _unitOfWork.UserRepository
            .FindOneOrDefaultAsync(user => user.Email == query.Email)
            ?? throw new BadRequestException(defaultUserNotFoundErrorMessage);

        if (!_passwordHasher.Verify(query.Password, user.PasswordHash))
        {
            throw new BadRequestException(defaultUserNotFoundErrorMessage);
        }


        var token = await _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
