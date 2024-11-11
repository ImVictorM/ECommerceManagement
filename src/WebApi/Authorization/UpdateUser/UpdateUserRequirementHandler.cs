using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApi.Common.Extensions;

namespace WebApi.Authorization.UpdateUser;

/// <summary>
/// Requirement handler for the <see cref="UpdateUserRequirement"/> requirement.
/// </summary>
public class UpdateUserRequirementHandler : AuthorizationHandler<UpdateUserRequirement>
{
    private readonly ISender _sender;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateUserRequirementHandler"/> class.
    /// </summary>
    /// <param name="sender">Mediatr sender.</param>
    public UpdateUserRequirementHandler(ISender sender)
    {
        _sender = sender;
    }

    /// <inheritdoc/>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UpdateUserRequirement requirement)
    {
        if (context.Resource is not HttpContext httpContext)
        {
            return;
        }

        var userToUpdateId = httpContext.GetRouteValue("id")?.ToString();

        if (userToUpdateId == null)
        {
            return;
        }

        if (context.User.GetId() != userToUpdateId)
        {

            if (!context.User.IsAdmin())
            {
                return;
            }

            var userToUpdateQueryResponse = await _sender.Send(new GetUserByIdQuery(userToUpdateId));

            if (userToUpdateQueryResponse.User.IsAdmin())
            {
                return;
            }
        }

        context.Succeed(requirement);
    }
}
