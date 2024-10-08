using MediatR;
using Application.Authentication.Common;
using Application.Common.Errors;
using Application.Common.Interfaces.Authentication;
using Domain.UserAggregate;
using Domain.RoleAggregate.Enums;
using Application.Common.Interfaces.Persistence;
using Domain.RoleAggregate;
using Microsoft.Extensions.Logging;
using Domain.Common.ValueObjects;

namespace Application.Authentication.Commands.Register;

/// <summary>
/// Command handler for the command <see cref="RegisterCommand"/>.
/// Handles user registration.
/// </summary>
public partial class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
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
    /// <param name="logger">The register logger.</param>
    public RegisterCommandHandler(
        IJwtTokenService jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ILogger<RegisterCommandHandler> logger
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    /// <summary>
    /// Handle user registration.
    /// </summary>
    /// <param name="command">The command that will trigger the registration process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An authentication result including the authentication token.</returns>
    /// <exception cref="BadRequestException">Exception in case the user already exists.</exception>
    public async Task<AuthenticationResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        LogHandlingRegisterCommand(command.Email);

        var inputEmail = Email.Create(command.Email);

        if (await _unitOfWork.UserRepository.FindOneOrDefaultAsync(user => user.Email == inputEmail) is not null)
        {
            LogUserAlreadyExists(command.Email);
            throw new BadRequestException("User already exists.");
        }

        var customerRoleName = Role.ToName(RoleTypes.CUSTOMER);
        var customerRole = await _unitOfWork.RoleRepository.FindOneOrDefaultAsync(role => role.Name == customerRoleName);

        if (customerRole == null)
        {
            LogFailedToFetchCustomerRole();
            throw new HttpException($"Couldn't find the role with name {customerRoleName}");
        }

        var (passwordHash, passwordSalt) = _passwordHasher.Hash(command.Password);

        var user = User.Create(
            command.Name,
            inputEmail,
            passwordHash,
            passwordSalt,
            customerRole.Id
        );

        LogUserCreatedWithCustomerRole();

        await _unitOfWork.UserRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        LogUserSavedSuccessfully(user.Email.Value);

        var token = await _jwtTokenGenerator.GenerateTokenAsync(user);

        LogTokenGeneratedSuccessfully();

        return new AuthenticationResult(user, token);
    }
}
