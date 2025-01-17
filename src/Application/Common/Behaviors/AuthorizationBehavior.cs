using Application.Common.Errors;
using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Requests;

using MediatR;
using System.Reflection;

namespace Application.Common.Behaviors;

/// <summary>
/// A pipeline behavior that performs authorization before the request is handled.
/// This behavior ensures that the current user has the necessary privileges before being passed
/// to the next handler in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : RequestWithAuthorization<TResponse>
{
    private readonly IAuthorizationService _authorizationService;

    internal AuthorizationBehavior(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var authorizationAttributes = request
            .GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var requiredRoles = authorizationAttributes
            .Select(attr => attr.RoleName)
            .Where(roleName => !string.IsNullOrWhiteSpace(roleName))
            .Cast<string>()
            .ToList();

        var requiredPolicies = authorizationAttributes
            .Select(attr => attr.PolicyType)
            .Where(policyType => policyType != null)
            .Cast<Type>()
            .ToList();

        var isAuthorized = await _authorizationService.IsCurrentUserAuthorizedAsync(request, requiredRoles, requiredPolicies);

        if (isAuthorized)
        {
            return await next();
        }

        throw new NotAllowedException();
    }
}
