using MediatR;
using Application.Authentication.Common;
using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Authentication;
using Domain.UserAggregate;

namespace Application.Authentication.Commands.Register;

/// <summary>
/// Command handler for the command <see cref="RegisterCommand"/>.
/// Handles user registration.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    /// <summary>
    /// Token service to generate authentication tokens.
    /// </summary>
    private readonly IJwtTokenService _jwtTokenGenerator;
    /// <summary>
    /// User repository to interact and persist user data.
    /// </summary>
    private readonly IUserRepository _userRepository;
    /// <summary>
    /// Role repository to assign roles to users.
    /// </summary>
    private readonly IRoleRepository _roleRepository;
    /// <summary>
    /// Hash service to hash and verify passwords.
    /// </summary>
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">Token service.</param>
    /// <param name="userRepository">User repository.</param>
    /// <param name="roleRepository">Role repository.</param>
    /// <param name="passwordHasher">Password hash service.</param>
    public RegisterCommandHandler(
        IJwtTokenService jwtTokenGenerator,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// Handle user registration.
    /// </summary>
    /// <param name="command">The command that will trigger the registration process.</param>
    /// <param name="cancellationToken">The cancelation token.</param>
    /// <returns>An authentication result including the authentication token.</returns>
    /// <exception cref="BadRequestException">Exception in case the user already exists.</exception>
    public async Task<AuthenticationResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // Checks if user already exists
        if (_userRepository.GetUserByEmailAddress(command.Email) is not null)
        {
            throw new BadRequestException("User already exists.");
        }

        // Create user
        User user = User.Create(
            command.Name,
            command.Email,
            _passwordHasher.Hash(command.Password)
        );

        await _userRepository.AddAsync(user);

        // Assign role to the user
        var roles = await _roleRepository.AssignRoleToUserAsync(user.Id.Value, "customer");

        // Generate the token
        var token = _jwtTokenGenerator.GenerateToken(user, roles);
        return new AuthenticationResult(user, token);
    }
}
