using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Users.Commands.DeactivateUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUserById;
using Carter;
using Contracts.Users;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization.AdminRequired;
using WebApi.Common.Extensions;

namespace WebApi.Endpoints;

/// <summary>
/// Defines all user-related endpoints.
/// </summary>
public sealed class UserEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var userGroup = app
            .MapGroup("users")
            .WithTags("Users")
            .WithOpenApi();

        userGroup
            .MapGet("/self", GetUserByAuthenticationToken)
            .WithName("GetUserByAuthenticationToken")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get User By Authentication Token",
                Description = "Retrieves the currently authenticated user's details. Requires authentication.",
            })
            .RequireAuthorization();

        userGroup
            .MapGet("/{id:long}", GetUserById)
            .WithName("GetUserById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get User By Id",
                Description = "Retrieves a specific user's details by identifier. Admin authentication is required.",
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        userGroup
            .MapGet("/", GetAllUsers)
            .WithName("GetAllUsers")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get All Users",
                Description = "Retrieves the users. The {{active}} query parameter is optional and can be used to filter active/inactive users. Admin authentication is required."
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        userGroup
            .MapPut("/{id:long}", UpdateUser)
            .WithName("UpdateUserById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update User By Id",
                Description = "Updates a user's details. Users can only update their own details. Administrators can update any other non-administrator user's details. Requires authentication."
            })
            .RequireAuthorization();

        userGroup
            .MapDelete("/{id:long}", DeactivateUser)
            .WithName("DeactivateUser")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete User",
                Description = "Deactivates a user by setting them as inactive. Users can deactivate their accounts, while administrators can deactivate any non-administrator user's account."
            })
            .RequireAuthorization();
    }

    private async Task<Results<Ok<UserResponse>, BadRequest, UnauthorizedHttpResult>> GetUserByAuthenticationToken(
        IHttpContextAccessor httpContextAccessor,
        ISender sender,
        IMapper mapper
    )
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userId == null)
        {
            return TypedResults.Unauthorized();
        }

        var query = new GetUserByIdQuery(userId);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserResponse>(result));
    }

    private async Task<Results<Ok<UserResponse>, BadRequest, ForbidHttpResult, UnauthorizedHttpResult>> GetUserById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetUserByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserResponse>(result));
    }

    private async Task<Results<Ok<UserListResponse>, ForbidHttpResult, UnauthorizedHttpResult>> GetAllUsers(
        [FromQuery(Name = "active")] bool? IsActive,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetAllUsersQuery(IsActive);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserListResponse>(result));
    }

    private async Task<Results<NoContent, BadRequest, Conflict, ForbidHttpResult, UnauthorizedHttpResult>> UpdateUser(
        [FromRoute] string id,
        [FromBody] UpdateUserRequest request,
        ISender sender,
        IMapper mapper,
        IHttpContextAccessor contextAccessor
    )
    {
        var idAuthenticatedUser = contextAccessor.HttpContext!.User.GetId();

        var command = mapper.Map<UpdateUserCommand>((idAuthenticatedUser, id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    private async Task<Results<NoContent, BadRequest, ForbidHttpResult, UnauthorizedHttpResult>> DeactivateUser(
        [FromRoute] string id,
        ISender sender,
        IHttpContextAccessor contextAccessor
    )
    {
        var idUserAuthenticated = contextAccessor.HttpContext!.User.GetId();

        var command = new DeactivateUserCommand(idUserAuthenticated, id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
