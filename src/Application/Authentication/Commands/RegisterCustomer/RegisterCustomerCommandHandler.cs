using Domain.UserAggregate;
using Domain.UserAggregate.Specification;

using Application.Common.Security.Authentication;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Common.Security.Identity;
using Application.Common.Errors;
using Application.Authentication.DTOs.Results;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Authentication.Commands.RegisterCustomer;

internal sealed partial class RegisterCustomerCommandHandler
    : IRequestHandler<RegisterCustomerCommand, AuthenticationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

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

    public async Task<AuthenticationResult> Handle(
        RegisterCustomerCommand command,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCustomerRegistration(command.Email);

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

        await _userRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        LogCustomerCreatedSuccessfully();

        var userIdentity = new IdentityUser(
                user.Id.ToString(),
                user.UserRoles.Select(ur => ur.Role).ToList()
        );

        var token = _jwtTokenService.GenerateToken(userIdentity);

        LogAuthenticationTokenGenerated();

        LogRegistrationCompletedSuccessfully();

        return AuthenticationResult.FromUserWithToken(
            user,
            token
        );
    }
}
