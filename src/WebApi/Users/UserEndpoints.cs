using Application.Users.Commands.DeactivateUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetSelf;
using Application.Users.Queries.GetUsers;
using Application.Users.DTOs.Filters;

using Contracts.Users;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.Users;

/// <summary>
/// Provides endpoints for the user features.
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
            .WithName(nameof(GetUserByAuthenticationToken))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get User By Authentication Token",
                Description =
                "Retrieves the authenticated user details. " +
                "Authentication is required.",
            })
            .RequireAuthorization();

        userGroup
            .MapGet("/{id:long}", GetUserById)
            .WithName(nameof(GetUserById))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get User By Id",
                Description =
                "Retrieves a user by its identifier. " +
                "Admin authentication required.",
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
            .MapGet("/", GetUsers)
            .WithName(nameof(GetUsers))
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
            .WithName(nameof(UpdateUser))
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
            .WithName(nameof(DeactivateUser))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Deactivate User",
                Description =
                "Deactivates a user by setting them as inactive. " +
                "Users can deactivate their accounts, while administrators can " +
                "deactivate any non-administrator user's account.",
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

    internal async Task<Results<
        Ok<UserResponse>,
        NotFound,
        UnauthorizedHttpResult
    >> GetUserByAuthenticationToken(
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetSelfQuery();

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserResponse>(result));
    }

    internal async Task<Results<
        Ok<UserResponse>,
        NotFound,
        ForbidHttpResult,
        UnauthorizedHttpResult
    >> GetUserById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetUserByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<UserResponse>(result));
    }

    internal async Task<Results<
        Ok<List<UserResponse>>,
        ForbidHttpResult,
        UnauthorizedHttpResult
    >> GetUsers(
        [FromQuery(Name = "active")] bool? IsActive,
        ISender sender,
        IMapper mapper
    )
    {
        var filters = new UserFilters(IsActive);

        var query = new GetUsersQuery(filters);

        var result = await sender.Send(query);

        var response = result
            .Select(mapper.Map<UserResponse>)
            .ToList();

        return TypedResults.Ok(response);
    }

    internal async Task<Results<
        NoContent,
        BadRequest,
        Conflict,
        ForbidHttpResult,
        UnauthorizedHttpResult
    >> UpdateUser(
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

    internal async Task<Results<
        NoContent,
        ForbidHttpResult,
        NotFound,
        UnauthorizedHttpResult
    >> DeactivateUser(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new DeactivateUserCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
