using Application.Authentication.Common;
using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Persistence;
using Domain.Users;
using MediatR;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Query handler for the <see cref="LoginQuery"/> query.
/// Handles user authentication.
/// </summary>
public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    /// <summary>
    /// User repository to interact and persist user data.
    /// </summary>
    private readonly IUserRepository _userRepository;
    /// <summary>
    /// Service to hash and verify passwords.
    /// </summary>
    private readonly IPasswordHasher _passwordHasher;
    /// <summary>
    /// Service to generate authentication tokens.
    /// </summary>
    private readonly IJwtTokenService _jwtTokenGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginQueryHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">Token service.</param>
    /// <param name="userRepository">User repository.</param>
    /// <param name="passwordHasher">Password hash service.</param>
    public LoginQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenGenerator
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    /// <summary>
    /// Handle user authentication.
    /// </summary>
    /// <param name="query">The query that triggers the authentication process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user with authentication token.</returns>
    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        string defaultErrorMessage = "User email or password is incorrect.";

        User user = _userRepository.GetUserByEmailAddress(query.Email) ?? throw new BadRequestException(defaultErrorMessage);

        if (!_passwordHasher.Verify(query.Password, user.PasswordHash))
        {
            throw new BadRequestException(defaultErrorMessage);
        }

        string token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}