using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Constants.Policies;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUserById;
using Carter;
using Contracts.Users;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

/// <summary>
/// Define all user related endpoints.
/// </summary>
public sealed class UserEndpoints : ICarterModule
{
    /// <summary>
    /// The base endpoint for the user related endpoints.
    /// </summary>
    public const string BaseEndpoint = "users";

    /// <summary>
    /// Add the routes related to users.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(BaseEndpoint);

        userGroup.MapGet("/self", GetUserByAuthenticationToken).RequireAuthorization();
        userGroup.MapGet("/{id:long}", GetUserById).RequireAuthorization(PolicyConstants.Admin.Name);
        userGroup.MapGet("/", GetAllUsers).RequireAuthorization(PolicyConstants.Admin.Name);
    }

    /// <summary>
    /// Gets a user based on their authentication token.
    /// </summary>
    /// <param name="httpContextAccessor">The http context to get the user identity.</param>
    /// <param name="sender">The mediatr sender.</param>
    /// <param name="mapper">The mapper.</param>
    /// <returns>The identified user.</returns>
    private async Task<IResult> GetUserByAuthenticationToken(
        IHttpContextAccessor httpContextAccessor,
        ISender sender,
        IMapper mapper
    )
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userId == null)
        {
            return Results.Unauthorized();
        }

        var query = new GetUserByIdQuery(userId);

        var result = await sender.Send(query);

        return Results.Ok(mapper.Map<UserResponse>(result));
    }

    /// <summary>
    /// Gets a user by the identifier.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="sender">The mediatr sender.</param>
    /// <param name="mapper">The mapper.</param>
    /// <returns>An OK result containing the user.</returns>
    private async Task<IResult> GetUserById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetUserByIdQuery(id);

        var result = await sender.Send(query);

        return Results.Ok(mapper.Map<UserResponse>(result));
    }

    private async Task<IResult> GetAllUsers(
        [FromQuery(Name = "active")] bool? IsActive,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetAllUsersQuery(IsActive);

        var result = await sender.Send(query);

        return Results.Ok(mapper.Map<UserListResponse>(result));
    }
}
