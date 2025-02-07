using Application.Authentication.DTOs;
using Application.Authentication.Errors;
using Application.Common.Persistence;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using Domain.CarrierAggregate;

using SharedKernel.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Queries.LoginCarrier;

/// <summary>
/// handles the <see cref="LoginCarrierQuery"/> query.
/// </summary>
public sealed partial class LoginCarrierQueryHandler : IRequestHandler<LoginCarrierQuery, AuthenticationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="passwordHasher">The password hasher.</param>
    /// <param name="jwtTokenService">The jwt token service.</param>
    /// <param name="logger">The logger.</param>
    public LoginCarrierQueryHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ILogger<LoginCarrierQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(LoginCarrierQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingCarrierAuthentication();

        var carrierEmail = Email.Create(request.Email);

        var carrier = await _unitOfWork.CarrierRepository.FindOneOrDefaultAsync(c => c.Email == carrierEmail);

        if (carrier == null || !_passwordHasher.Verify(request.Password, carrier.PasswordHash))
        {
            LogCarrierAuthenticationFailed();
            throw new AuthenticationFailedException();
        }

        LogGeneratingCarrierAuthenticationToken(carrier.Id.ToString());
        var token = GenerateToken(carrier);

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
    private string GenerateToken(Carrier carrier)
    {
        var userIdentity = new IdentityUser(carrier.Id.ToString(), [carrier.Role]);

        var token = _jwtTokenService.GenerateToken(userIdentity);

        return token;
    }
}
