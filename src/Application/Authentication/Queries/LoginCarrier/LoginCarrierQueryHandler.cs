using Application.Authentication.DTOs;
using Application.Authentication.Errors;
using Application.Common.Persistence;
using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using Domain.CarrierAggregate;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Authentication.Queries.LoginCarrier;

/// <summary>
/// handles the <see cref="LoginCarrierQuery"/> query.
/// </summary>
public sealed class LoginCarrierQueryHandler : IRequestHandler<LoginCarrierQuery, AuthenticationResult>
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
    public LoginCarrierQueryHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    /// <inheritdoc/>
    public async Task<AuthenticationResult> Handle(LoginCarrierQuery request, CancellationToken cancellationToken)
    {
        var carrierEmail = Email.Create(request.Email);

        var carrier = await _unitOfWork.CarrierRepository.FindOneOrDefaultAsync(c => c.Email == carrierEmail);

        if (carrier == null || !_passwordHasher.Verify(request.Password, carrier.PasswordHash))
        {
            throw new AuthenticationFailedException();
        }

        var token = GenerateToken(carrier);

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
