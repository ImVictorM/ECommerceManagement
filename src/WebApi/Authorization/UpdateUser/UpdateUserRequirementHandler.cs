using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Authorization;

namespace WebApi.Authorization.UpdateUser;

/// <summary>
/// Requirement handler for the <see cref="UpdateUserRequirement"/>.
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
    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, UpdateUserRequirement requirement)
    {
        var authenticatedUserId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (context.Resource is not HttpContext httpContext)
        {
            return;
        }

        var userToUpdateId = httpContext.GetRouteValue("id")?.ToString();

        if (userToUpdateId == null)
        {
            return;
        }

        if (authenticatedUserId != userToUpdateId)
        {
            var authenticatedUserRoleNames= context.User
                .FindAll(ClaimTypes.Role)
                .Select(claim => claim.Value);

            var authenticatedUserIsAdmin = Role.HasAdminRole(authenticatedUserRoleNames);

            if (!authenticatedUserIsAdmin)
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
