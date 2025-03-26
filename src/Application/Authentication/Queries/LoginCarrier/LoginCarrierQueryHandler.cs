using Domain.CarrierAggregate;

using Application.Authentication.DTOs.Results;
using Application.Authentication.Errors;
using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Authentication.Queries.LoginCarrier;

internal sealed partial class LoginCarrierQueryHandler
    : IRequestHandler<LoginCarrierQuery, AuthenticationResult>
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

    public async Task<AuthenticationResult> Handle(
        LoginCarrierQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCarrierAuthentication();

        var carrierEmail = Email.Create(request.Email);

        var carrierFound = await _carrierRepository.FindByEmail(
            carrierEmail,
            cancellationToken
        );

        if (!IsCarrierNotNullAndPasswordIsCorrect(
            carrierFound,
            request.Password,
            out var carrier
        ))
        {
            LogCarrierAuthenticationFailed();
            throw new AuthenticationFailedException();
        }

        var carrierId = carrier.Id.ToString();

        LogGeneratingCarrierAuthenticationToken(carrierId);

        var userIdentity = new IdentityUser(carrierId, [carrier.Role]);

        var token = _jwtTokenService.GenerateToken(userIdentity);

        LogCarrierAuthenticatedSuccessfully();

        return AuthenticationResult.FromUserWithToken(
            carrier,
            token
        );
    }

    private bool IsCarrierNotNullAndPasswordIsCorrect(
        Carrier? carrierNullable,
        string password,
        out Carrier carrier
    )
    {
        if (carrierNullable is null)
        {
            carrier = null!;
            return false;
        }

        carrier = carrierNullable;

        return _passwordHasher.Verify(password, carrier.PasswordHash);
    }
}
