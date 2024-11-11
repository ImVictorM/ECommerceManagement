using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApi.Common.Extensions;

namespace WebApi.Authorization.DeactivateUser;

/// <summary>
/// Requirement handler for the <see cref="DeactivateUserRequirement"/> requirement.
/// </summary>
public class DeactivateUserRequirementHandler : AuthorizationHandler<DeactivateUserRequirement>
{
    private readonly ISender _sender;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserRequirementHandler"/> class.
    /// </summary>
    /// <param name="sender">The mediator sender.</param>
    public DeactivateUserRequirementHandler(ISender sender)
    {
        _sender = sender;
    }

    /// <inheritdoc/>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DeactivateUserRequirement requirement)
    {
        if (context.Resource is not HttpContext httpContext)
        {
            return;
        }

        var userToDeactivateId = httpContext.GetRouteValue("id")?.ToString();

        if (userToDeactivateId == null)
        {
            return;
        }

        if (context.User.GetId() != userToDeactivateId)
        {
            // Only admins are allowed to update other users that are not admins
            if (!context.User.IsAdmin())
            {
                return;
            }

            var userToDeactivate = await _sender.Send(new GetUserByIdQuery(userToDeactivateId));

            if (userToDeactivate.User.IsAdmin())
            {
                return;
            }
        }
        else
        {
            // Admins trying to deactivate themselves is not allowed
            if (context.User.IsAdmin())
            {
                return;
            }
        }

        context.Succeed(requirement);
    }
}
