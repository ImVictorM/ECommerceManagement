using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
using WebApi.Authorization.UpdateUser;

namespace WebApi.Endpoints;

/// <summary>
/// Define all user-related endpoints.
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
                Description = "Retrieves the user based on their authentication token",
            })
            .RequireAuthorization();

        userGroup
            .MapGet("/{id:long}", GetUserById)
            .WithName("GetUserById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get User By Id",
                Description = "Retrieves a user by their identifier",
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        userGroup
            .MapGet("/", GetAllUsers)
            .WithName("GetAllUsers")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get All Users",
                Description = "Retrieves all users, optionally filtered by active status"
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        userGroup
            .MapPut("/{id:long}", UpdateUser)
            .WithName("UpdateUserById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update User By Id",
                Description = " Updates a user's information"
            })
            .RequireAuthorization(UpdateUserPolicy.Name);
    }

    private async Task<Results<Ok<UserResponse>, UnauthorizedHttpResult, BadRequest>> GetUserByAuthenticationToken(
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

    private async Task<Results<Ok<UserResponse>, BadRequest>> GetUserById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetUserByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserResponse>(result));
    }

    private async Task<Ok<UserListResponse>> GetAllUsers(
        [FromQuery(Name = "active")] bool? IsActive,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetAllUsersQuery(IsActive);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserListResponse>(result));
    }

    private async Task<Results<Ok, BadRequest, Conflict>> UpdateUser(
        [FromRoute] string id,
        [FromBody] UpdateUserRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<UpdateUserCommand>((request, id));

        await sender.Send(command);

        return TypedResults.Ok();
    }
}
