using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Authentication;
using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Common.Errors;
using Application.Authentication.DTOs;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Authentication.Commands.Register;

/// <summary>
/// Command handler for the command <see cref="RegisterCommand"/>.
/// Handles user registration.
/// </summary>
public partial class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
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
        _jwtTokenService = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        LogHandlingRegisterCommand(command.Email);

        var inputEmail = Email.Create(command.Email);

        if (await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryUserByEmailSpecification(inputEmail)) is not null)
        {
            LogUserAlreadyExists();

            throw new EmailConflictException()
                .WithContext("Email", command.Email);
        }

        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = User.Create(
            command.Name,
            inputEmail,
            passwordHash,
            new HashSet<UserRole>()
            {
                UserRole.Create(Role.Customer.Id)
            }
        );

        LogUserCreatedWithCustomerRole();

        await _unitOfWork.UserRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        LogUserSavedSuccessfully(user.Email.Value);

        var availableRoles = Role.List().ToDictionary(r => r.Id);

        var token = _jwtTokenService.GenerateToken(
            new IdentityUser(
                user.Id.ToString(),
                user.UserRoles.Select(r => availableRoles[r.RoleId].Name).ToList()
            )
        );

        LogTokenGeneratedSuccessfully();

        return new AuthenticationResult(user, token);
    }
}
