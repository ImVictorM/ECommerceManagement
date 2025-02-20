using Domain.UserAggregate;
using Domain.UserAggregate.Specification;

using Application.Common.Security.Authentication;
using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Common.Errors;
using Application.Authentication.DTOs;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Authentication.Commands.RegisterCustomer;

/// <summary>
/// Command handler for the command <see cref="RegisterCustomerCommand"/>.
/// Handles user registration.
/// </summary>
public partial class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, AuthenticationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCustomerCommandHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">Token service.</param>
    /// <param name="passwordHasher">Password hash service.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The register logger.</param>
    public RegisterCustomerCommandHandler(
        IJwtTokenService jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        ILogger<RegisterCustomerCommandHandler> logger
    )
    {
        _jwtTokenService = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(
        RegisterCustomerCommand command,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingRegisterCustomer(command.Email);

        var inputEmail = Email.Create(command.Email);

        var emailIsInUse = await _userRepository.FindFirstSatisfyingAsync(
            new QueryUserByEmailSpecification(inputEmail),
            cancellationToken
        ) is not null;

        if (emailIsInUse)
        {
            LogEmailAlreadyInUse();

            throw new EmailConflictException().WithContext("Email", command.Email);
        }

        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = User.CreateCustomer(
            command.Name,
            inputEmail,
            passwordHash
        );

        LogCustomerCreated();

        await _userRepository.AddAsync(user);

        var userIdentity = new IdentityUser(
                user.Id.ToString(),
                user.UserRoles.Select(ur => ur.Role).ToList()
        );

        var token = _jwtTokenService.GenerateToken(userIdentity);

        LogAuthenticationTokenGenerated();

        await _unitOfWork.SaveChangesAsync();

        LogRegistrationCompleteSuccessfully();

        return new AuthenticationResult(
            new AuthenticatedIdentity(
                user.Id.ToString(),
                user.Name,
                user.Email.ToString(),
                user.Phone
            ),
            token
        );
    }
}
