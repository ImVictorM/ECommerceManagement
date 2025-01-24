using Application.Authentication.Commands.Register;
using Application.Authentication.Queries.Login;

using Carter;
using Contracts.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Endpoints;

/// <summary>
/// Wraps the routes related to authentication.
/// </summary>
public sealed class AuthenticationEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var authenticationGroup = app
            .MapGroup("/auth")
            .WithTags("Authentication")
            .WithOpenApi();

        authenticationGroup
            .MapPost("/register", Register)
            .WithName("Register")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Register",
                Description = "This endpoint allows a new user to register for an account."
            });

        authenticationGroup
            .MapPost("/login", Login)
            .WithName("Login")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Login",
                Description = "Users can log in using their email and password."
            });
    }

    private async Task<Results<Created<AuthenticationResponse>, BadRequest, Conflict>> Register(
        RegisterRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<RegisterCommand>(request);

        var result = await sender.Send(command);

        var mappedResult = mapper.Map<AuthenticationResponse>(result);

        return TypedResults.Created($"/users/{mappedResult.Id}", mappedResult);
    }

    private async Task<Results<Ok<AuthenticationResponse>, BadRequest>> Login(
        LoginRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var query = mapper.Map<LoginQuery>(request);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<AuthenticationResponse>(result));
    }
}
