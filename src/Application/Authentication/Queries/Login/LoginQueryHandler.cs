using Application.Authentication.Common.DTOs;
using Application.Authentication.Common.Errors;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate;
using Domain.UserAggregate.Specification;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.ValueObjects;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Query handler for the <see cref="LoginQuery"/> query.
/// Handles user authentication.
/// </summary>
public partial class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginQueryHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">Token service.</param>
    /// <param name="passwordHasher">Password hash service.</param>
    /// <param name="unitOfWork">The unity of work.</param>
    /// <param name="logger">The query handler logger.</param>
    public LoginQueryHandler(
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenGenerator,
        IUnitOfWork unitOfWork,
        ILogger<LoginQueryHandler> logger
    )
    {
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var inputEmail = Email.Create(query.Email);

        LogHandlingLoginQuery(inputEmail.Value);

        var user = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(
            new QueryUserByEmailSpecification(inputEmail).And(new QueryActiveUserSpecification())
        );

        if (user == null || !IsUserPasswordCorrect(query, user))
        {
            LogAuthenticationFailed();

            throw new AuthenticationFailedException();
        }

        LogSuccessfullyAuthenticatedUser(query.Email);

        var token = _jwtTokenGenerator.GenerateToken(user);
        LogTokenGenerated();

        return new AuthenticationResult(user, token);
    }

    private bool IsUserPasswordCorrect(LoginQuery query, User user)
    {
        return _passwordHasher.Verify(query.Password, user.PasswordHash.GetHashPart(), user.PasswordHash.GetSaltPart());
    }
}
