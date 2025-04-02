using Application.Common.Errors;
using Application.Common.Security.Authorization.Requests;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Common.Security.Authorization;

internal sealed partial class AuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequestWithAuthorization<TResponse>
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationBehavior(
        IAuthorizationService authorizationService,
        ILogger<AuthorizationBehavior<TRequest, TResponse>> logger
    )
    {
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        LogAuthorizingRequest(typeof(TRequest).Name);

        var authorizationMetadata = AuthorizeAttribute.GetAuthorizationMetadata(
            request.GetType()
        );

        LogRequiredRolesQuantity(authorizationMetadata.Roles.Count);
        LogRequiredPoliciesQuantity(authorizationMetadata.Policies.Count);

        LogCheckingUserAuthorization();

        var isAuthorized = await _authorizationService.IsCurrentUserAuthorizedAsync(
            request,
            authorizationMetadata,
            cancellationToken
        );

        if (isAuthorized)
        {
            LogUserIsAuthorized();
            return await next();
        }

        LogUserIsNotAuthorized();
        throw new NotAllowedException();
    }
}
