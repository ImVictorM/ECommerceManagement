using Application.Authentication.DTOs.Results;
using Application.Authentication.Errors;
using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using Domain.UserAggregate;
using Domain.UserAggregate.Specification;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Authentication.Queries.LoginUser;

internal sealed partial class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, AuthenticationResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserRepository _userRepository;

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

    public async Task<AuthenticationResult> Handle(
        LoginUserQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingUserAuthentication(request.Email);

        var inputEmail = Email.Create(request.Email);

        var userQuerySpecification =
            new QueryUserByEmailSpecification(inputEmail)
            .And(new QueryActiveUserSpecification());

        var userFound = await _userRepository.FindFirstSatisfyingAsync(
            userQuerySpecification,
            cancellationToken
        );

        if (!IsUserNotNullAndPasswordIsCorrect(
            userFound,
            request.Password,
            out var user
        ))
        {
            LogUserAuthenticationFailed();

            throw new AuthenticationFailedException();
        }

        LogGeneratingUserAuthenticationToken();

        var userId = user.Id.ToString();

        var userIdentity = new IdentityUser(
            userId,
            user.UserRoles.Select(ur => ur.Role).ToList()
        );

        var token = _jwtTokenService.GenerateToken(userIdentity);

        LogUserAuthenticatedSuccessfully(request.Email);

        return AuthenticationResult.FromUserWithToken(
            user,
            token
        );
    }

    private bool IsUserNotNullAndPasswordIsCorrect(
        User? userNullable,
        string password,
        out User user
    )
    {
        if (userNullable is null)
        {
            user = null!;
            return false;
        }

        user = userNullable;

        return _passwordHasher.Verify(password, user.PasswordHash);
    }
}
