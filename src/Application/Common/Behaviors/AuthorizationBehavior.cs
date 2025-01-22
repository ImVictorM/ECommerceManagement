using Application.Common.Errors;
using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Requests;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Application.Common.Behaviors;

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

        var authorizationAttributes = request
            .GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            LogNoAuthorizationAttributesFound();

            return await next();
        }

        var requiredRoles = authorizationAttributes
            .Select(attr => attr.RoleName)
            .Where(roleName => !string.IsNullOrWhiteSpace(roleName))
            .Cast<string>()
            .ToList();

        LogRequiredRoles(requiredRoles.Count);

        var requiredPolicies = authorizationAttributes
            .Select(attr => attr.PolicyType)
            .Where(policyType => policyType != null)
            .Cast<Type>()
            .ToList();

        LogRequiredPolicies(requiredRoles.Count);

        LogCheckingUserAuthorization();

        var isAuthorized = await _authorizationService.IsCurrentUserAuthorizedAsync(request, requiredRoles, requiredPolicies);

        if (isAuthorized)
        {
            LogUserIsAuthorized();
            return await next();
        }

        LogUserIsNotAuthorized();
        throw new NotAllowedException();
    }
}
