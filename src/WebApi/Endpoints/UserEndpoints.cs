using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUserById;
using Carter;
using Contracts.Users;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization.AdminRequired;
using WebApi.Authorization.UpdateUser;

namespace WebApi.Endpoints;

/// <summary>
/// Define all user-related endpoints.
/// </summary>
public sealed class UserEndpoints : ICarterModule
{
    /// <summary>
    /// The base endpoint for the user-related endpoints.
    /// </summary>
    public const string BaseEndpoint = "users";

    /// <summary>
    /// Add the routes related to user operations.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup(BaseEndpoint);

        userGroup.MapGet("/self", GetUserByAuthenticationToken).RequireAuthorization();
        userGroup.MapGet("/{id:long}", GetUserById).RequireAuthorization(AdminRequiredPolicy.Name);
        userGroup.MapGet("/", GetAllUsers).RequireAuthorization(AdminRequiredPolicy.Name);
        userGroup.MapPut("/{id:long}", UpdateUser).RequireAuthorization(UpdateUserPolicy.Name);
    }

    /// <summary>
    /// Retrieves the user based on their authentication token.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor used to obtain the user identity.</param>
    /// <param name="sender">The mediator sender for sending queries and commands.</param>
    /// <param name="mapper">The mapper for mapping query results to response models.</param>
    /// <returns>A result containing the identified user or an unauthorized response.</returns>
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
    /// Retrieves a user by their identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="sender">The mediator sender for sending queries and commands.</param>
    /// <param name="mapper">The mapper for mapping query results to response models.</param>
    /// <returns>An OK result containing the user or a not found response.</returns>
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

    /// <summary>
    /// Retrieves all users, optionally filtered by active status.
    /// </summary>
    /// <param name="IsActive">Optional parameter to filter users by their active status.</param>
    /// <param name="sender">The mediator sender for sending queries and commands.</param>
    /// <param name="mapper">The mapper for mapping query results to response models.</param>
    /// <returns>An OK result containing the list of users.</returns>
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

    /// <summary>
    /// Updates a user's information.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be updated.</param>
    /// <param name="request">The request object containing updated user information.</param>
    /// <param name="sender">The mediator sender for sending commands.</param>
    /// <param name="mapper">The mapper for mapping the request to command.</param>
    /// <returns>An OK result indicating the update was successful.</returns>
    private async Task<IResult> UpdateUser(
        [FromRoute] string id,
        [FromBody] UpdateUserRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<UpdateUserCommand>((request, id));

        await sender.Send(command);

        return Results.Ok();
    }
}
