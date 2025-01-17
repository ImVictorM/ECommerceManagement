using Domain.UserAggregate;
using Domain.UserAggregate.Specification;

using Application.Authentication.Common.DTOs;
using Application.Authentication.Common.Errors;
using Application.Common.Persistence;
using Application.Common.Security.Authentication;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;

using SharedKernel.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Query handler for the <see cref="LoginQuery"/> query.
/// Handles user authentication.
/// </summary>
public partial class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
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
        _jwtTokenService = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
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
        var availableRoles = Role.List().ToDictionary(r => r.Id);
        var userRoleNames = user.UserRoles.Select(ur => availableRoles[ur.RoleId].Name).ToList();

        var userIdentity = new IdentityUser(user.Id.ToString(), userRoleNames);

        var token = _jwtTokenService.GenerateToken(userIdentity);

        return token;
    }

    private bool IsUserPasswordCorrect(LoginQuery query, User user)
    {
        return _passwordHasher.Verify(query.Password, user.PasswordHash);
    }
}
