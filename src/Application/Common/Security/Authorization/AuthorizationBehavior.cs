using Application.Common.Errors;
using Application.Common.Security.Authorization.Requests;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Security.Authorization;

/// <summary>
/// A pipeline behavior that performs authorization before the request is handled.
/// This behavior ensures that the current user has the necessary privileges before being passed
/// to the next handler in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public partial class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequestWithAuthorization<TResponse>
{
    private readonly IAuthorizationService _authorizationService;

    /// <summary>
    /// Initiates a new instance of the <see cref="AuthorizationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="authorizationService">The authorization service.</param>
    /// <param name="logger">The logger.</param>
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
