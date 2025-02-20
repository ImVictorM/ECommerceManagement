using Domain.UserAggregate.Specification;

using Application.Authentication.DTOs;
using Application.Authentication.Errors;
using Application.Common.Persistence;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Authentication.Queries.LoginUser;

/// <summary>
/// Query handler for the <see cref="LoginUserQuery"/> query.
/// Handles user authentication.
/// </summary>
public partial class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthenticationResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginUserQueryHandler"/> class.
    /// </summary>
    /// <param name="jwtTokenService">The token service.</param>
    /// <param name="passwordHasher">The password hash service.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The query handler logger.</param>
    public LoginUserQueryHandler(
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IUserRepository userRepository,
        ILogger<LoginUserQueryHandler> logger
    )
    {
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(LoginUserQuery query, CancellationToken cancellationToken)
    {
        LogInitiatingUserLogin(query.Email);

        var inputEmail = Email.Create(query.Email);

        var userQuerySpecification = new QueryUserByEmailSpecification(inputEmail).And(new QueryActiveUserSpecification());

        var user = await _userRepository.FindFirstSatisfyingAsync(userQuerySpecification, cancellationToken);

        if (user == null || !_passwordHasher.Verify(query.Password, user.PasswordHash))
        {
            LogUserAuthenticationFailed();

            throw new AuthenticationFailedException();
        }

        LogGeneratingUserAuthenticationToken();

        var userIdentity = new IdentityUser(
            user.Id.ToString(),
            user.UserRoles.Select(ur => ur.Role).ToList()
        );

        var token = _jwtTokenService.GenerateToken(userIdentity);

        LogUserAuthenticatedSuccessfully(query.Email);

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
