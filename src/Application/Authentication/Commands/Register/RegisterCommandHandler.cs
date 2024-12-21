using MediatR;
using Application.Common.Errors;
using Application.Common.Interfaces.Authentication;
using Domain.UserAggregate;
using Application.Common.Interfaces.Persistence;
using Microsoft.Extensions.Logging;
using Application.Authentication.Common.DTOs;
using SharedKernel.ValueObjects;
using Domain.UserAggregate.Specification;

namespace Application.Authentication.Commands.Register;

/// <summary>
/// Command handler for the command <see cref="RegisterCommand"/>.
/// Handles user registration.
/// </summary>
public partial class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenGenerator;
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

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        LogHandlingRegisterCommand(command.Email);

        var inputEmail = Email.Create(command.Email);

        if (await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryUserByEmailSpecification(inputEmail)) is not null)
        {
            LogUserAlreadyExists();

            throw new UserAlreadyExistsException().WithContext("Email", command.Email);
        }

        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = User.Create(
            command.Name,
            inputEmail,
            passwordHash
        );

        LogUserCreatedWithCustomerRole();

        await _unitOfWork.UserRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        LogUserSavedSuccessfully(user.Email.Value);

        var token = _jwtTokenGenerator.GenerateToken(user);

        LogTokenGeneratedSuccessfully();

        return new AuthenticationResult(user, token);
    }
}
