using Application.Authentication.DTOs;
using Application.Authentication.Errors;
using Application.Common.Persistence;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Authentication.Queries.LoginCarrier;

/// <summary>
/// handles the <see cref="LoginCarrierQuery"/> query.
/// </summary>
public sealed partial class LoginCarrierQueryHandler : IRequestHandler<LoginCarrierQuery, AuthenticationResult>
{
    private readonly ICarrierRepository _carrierRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierQueryHandler"/> class.
    /// </summary>
    /// <param name="carrierRepository">The carrier repository.</param>
    /// <param name="passwordHasher">The password hasher.</param>
    /// <param name="jwtTokenService">The jwt token service.</param>
    /// <param name="logger">The logger.</param>
    public LoginCarrierQueryHandler(
        ICarrierRepository carrierRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ILogger<LoginCarrierQueryHandler> logger
    )
    {
        _carrierRepository = carrierRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(LoginCarrierQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingCarrierAuthentication();

        var carrierEmail = Email.Create(request.Email);

        var carrier = await _carrierRepository.FindByEmail(carrierEmail, cancellationToken);

        if (carrier == null || !_passwordHasher.Verify(request.Password, carrier.PasswordHash))
        {
            LogCarrierAuthenticationFailed();
            throw new AuthenticationFailedException();
        }

        LogGeneratingCarrierAuthenticationToken(carrier.Id.ToString());

        var userIdentity = new IdentityUser(carrier.Id.ToString(), [carrier.Role]);

        var token = _jwtTokenService.GenerateToken(userIdentity);

        LogCarrierAuthenticatedSuccessfully();

        return new AuthenticationResult(
            new AuthenticatedIdentity(
                carrier.Id.ToString(),
                carrier.Name,
                carrier.Email.ToString(),
                carrier.Phone
            ),
            token
        );
    }
}
