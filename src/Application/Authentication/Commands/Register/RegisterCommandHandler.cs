using MediatR;
using Application.Authentication.Common;
using Application.Common.Errors;
using Application.Common.Interfaces.Authentication;
using Domain.UserAggregate;
using Domain.RoleAggregate.Enums;
using Application.Common.Interfaces.Persistence;
using Domain.RoleAggregate;

namespace Application.Authentication.Commands.Register;

/// <summary>
/// Command handler for the command <see cref="RegisterCommand"/>.
/// Handles user registration.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    /// <summary>
    /// Component to interact with the repositories and persist changes.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;
    /// <summary>
    /// Token service to generate authentication tokens.
    /// </summary>
    private readonly IJwtTokenService _jwtTokenGenerator;
    /// <summary>
    /// Hash service to hash and verify passwords.
    /// </summary>
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">Token service.</param>
    /// <param name="passwordHasher">Password hash service.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public RegisterCommandHandler(
        IJwtTokenService jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
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
        if (await _unitOfWork.UserRepository.FindOneOrDefaultAsync(user => user.Email == command.Email) is not null)
        {
            throw new BadRequestException("User already exists.");
        }

        // Create user and assign role
        var user = User.Create(
            command.Name,
            command.Email,
            _passwordHasher.Hash(command.Password)
        );

        var customerRoleName = Role.ToName(RoleTypes.CUSTOMER);

        var customerRole = await _unitOfWork.RoleRepository
            .FindOneOrDefaultAsync(role => role.Name == customerRoleName)
            ?? throw new HttpException($"Couldn't find the role with name {customerRoleName}");

        user.AddUserRole(customerRole.Id);

        await _unitOfWork.UserRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        // Generate the token
        var token = await _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
