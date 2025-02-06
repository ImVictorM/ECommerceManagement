using Domain.UserAggregate;
using Domain.UserAggregate.Specification;

using Application.Authentication.DTOs;
using Application.Authentication.Errors;
using Application.Common.Persistence;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;
using Application.Common.Extensions;

using SharedKernel.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.LoginUser;

/// <summary>
/// Query handler for the <see cref="LoginUserQuery"/> query.
/// Handles user authentication.
/// </summary>
public partial class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthenticationResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginUserQueryHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">Token service.</param>
    /// <param name="passwordHasher">Password hash service.</param>
    /// <param name="unitOfWork">The unity of work.</param>
    /// <param name="logger">The query handler logger.</param>
    public LoginUserQueryHandler(
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenGenerator,
        IUnitOfWork unitOfWork,
        ILogger<LoginUserQueryHandler> logger
    )
    {
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(LoginUserQuery query, CancellationToken cancellationToken)
    {
        var inputEmail = Email.Create(query.Email);

        LogHandlingLoginQuery(inputEmail.Value);

        var userQuerySpecification = new QueryUserByEmailSpecification(inputEmail).And(new QueryActiveUserSpecification());

        var user = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(userQuerySpecification);

        if (user == null || !IsUserPasswordCorrect(query, user))
        {
            LogAuthenticationFailed();

            throw new AuthenticationFailedException();
        }

        var token = GenerateToken(user);

        LogSuccessfullyAuthenticatedUser(query.Email);

        return new AuthenticationResult(user, token);
    }

    private string GenerateToken(User user)
    {
        var userIdentity = new IdentityUser(user.Id.ToString(), user.UserRoles.GetRoleNames());

        var token = _jwtTokenService.GenerateToken(userIdentity);

        return token;
    }

    private bool IsUserPasswordCorrect(LoginUserQuery query, User user)
    {
        return _passwordHasher.Verify(query.Password, user.PasswordHash);
    }
}
