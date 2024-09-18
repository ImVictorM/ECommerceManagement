using Application.Authentication.Common;
using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Persistence;
using Domain.Users;
using MediatR;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Command handler for the command <see cref="LoginCommand"/>.
/// Handles user authentication.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
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
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">Token service.</param>
    /// <param name="userRepository">User repository.</param>
    /// <param name="passwordHasher">Password hash service.</param>
    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    /// <summary>
    /// Handle user authentication.
    /// </summary>
    /// <param name="command">The command that triggers the authentication process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user with authentication token.</returns>
    public async Task<AuthenticationResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        string defaultErrorMessage = "User email or password is incorrect.";

        User user = _userRepository.GetUserByEmailAddress(command.Email) ?? throw new BadRequestException(defaultErrorMessage);

        if (!_passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new BadRequestException(defaultErrorMessage);
        }

        string token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
