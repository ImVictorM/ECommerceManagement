using Application.Users.Commands.DeactivateUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetSelf;

using Contracts.Users;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.Users;

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
                Description = "Retrieves the authenticated user details. Authentication is required.",
            })
            .RequireAuthorization();

        userGroup
            .MapGet("/{id:long}", GetUserById)
            .WithName("GetUserById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get User By Id",
                Description = "Retrieves a user by its identifier. Admin authentication required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The user identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            })
            .RequireAuthorization();

        userGroup
            .MapGet("/", GetAllUsers)
            .WithName("GetAllUsers")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get All Users",
                Description = "Retrieves all the registered users. " +
                "Can receive an {{active}} query parameter to filter users. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "active",
                        In = ParameterLocation.Query,
                        Description = "Filters users by active boolean value.",
                        Required = false,
                        Schema = new() { Type = "boolean" }
                    }
                ],
            })
            .RequireAuthorization();

        userGroup
            .MapPut("/{id:long}", UpdateUser)
            .WithName("UpdateUserById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update User By Id",
                Description = "Updates a user by its identifier. " +
                "Users can only update their own details. " +
                "Administrators can update any other non-administrator user's details.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The user identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            })
            .RequireAuthorization();

        userGroup
            .MapDelete("/{id:long}", DeactivateUser)
            .WithName("DeactivateUser")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Deactivate User",
                Description = "Deactivates a user by setting them as inactive. " +
                "Users can deactivate their accounts, while administrators can deactivate any non-administrator user's account.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The user identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            })
            .RequireAuthorization();
    }

    private async Task<Results<Ok<UserResponse>, NotFound, UnauthorizedHttpResult>> GetUserByAuthenticationToken(
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetSelfQuery();

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserResponse>(result));
    }

    private async Task<Results<Ok<UserResponse>, NotFound, ForbidHttpResult, UnauthorizedHttpResult>> GetUserById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetUserByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserResponse>(result));
    }

    private async Task<Results<Ok<IEnumerable<UserResponse>>, ForbidHttpResult, UnauthorizedHttpResult>> GetAllUsers(
        [FromQuery(Name = "active")] bool? IsActive,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetAllUsersQuery(IsActive);

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<UserResponse>));
    }

    private async Task<Results<NoContent, BadRequest, Conflict, ForbidHttpResult, UnauthorizedHttpResult>> UpdateUser(
        [FromRoute] string id,
        [FromBody] UpdateUserRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<UpdateUserCommand>((id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    private async Task<Results<NoContent, ForbidHttpResult, UnauthorizedHttpResult>> DeactivateUser(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new DeactivateUserCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
