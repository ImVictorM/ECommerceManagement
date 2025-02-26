using Application.Authentication.DTOs;
using Application.Authentication.Errors;
using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Authentication.Queries.LoginCarrier;

internal sealed partial class LoginCarrierQueryHandler : IRequestHandler<LoginCarrierQuery, AuthenticationResult>
{
    private readonly ICarrierRepository _carrierRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

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
