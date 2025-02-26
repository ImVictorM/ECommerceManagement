using Application.Common.Errors;
using Application.Common.Security.Authorization.Requests;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Security.Authorization;

internal sealed partial class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
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

    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        LogAuthorizingRequest(typeof(TRequest).Name);

        var authorizationMetadata = AuthorizeAttribute.GetAuthorizationMetadata(request.GetType());

        LogRequiredRoles(authorizationMetadata.Roles.Count);
        LogRequiredPolicies(authorizationMetadata.Policies.Count);

        LogCheckingUserAuthorization();

        var isAuthorized = await _authorizationService.IsCurrentUserAuthorizedAsync(request, authorizationMetadata);

        if (isAuthorized)
        {
            LogUserIsAuthorized();
            return await next();
        }

        LogUserIsNotAuthorized();
        throw new NotAllowedException();
    }
}
