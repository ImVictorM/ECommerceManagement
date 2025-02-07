using Application.Authentication.Commands.RegisterCustomer;
using Application.Authentication.Queries.LoginUser;
using Application.Authentication.Queries.LoginCarrier;

using Contracts.Authentication;

using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Authentication;

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
            .MapPost("/register/users/customers", RegisterCustomer)
            .WithName("RegisterCustomer")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Register Customer",
                Description = "Allows a new customer user to register for an account."
            });

        authenticationGroup
            .MapPost("/login/users", LoginUser)
            .WithName("LoginUser")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Login User",
                Description = "Allows a user to log in using their email and password."
            });

        authenticationGroup
            .MapPost("/login/carriers", LoginCarrier)
            .WithName("LoginCarrier")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Login Carrier",
                Description = "Allows a carrier to log in using their email and password."
            });
    }

    private async Task<Results<Created<AuthenticationResponse>, BadRequest, Conflict>> RegisterCustomer(
        RegisterCustomerRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<RegisterCustomerCommand>(request);

        var result = await sender.Send(command);

        var authenticationResponse = mapper.Map<AuthenticationResponse>(result);

        return TypedResults.Created($"/users/{authenticationResponse.Id}", authenticationResponse);
    }

    private async Task<Results<Ok<AuthenticationResponse>, BadRequest>> LoginUser(
        LoginUserRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var query = mapper.Map<LoginUserQuery>(request);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<AuthenticationResponse>(result));
    }

    private async Task<Results<Ok<AuthenticationResponse>, BadRequest>> LoginCarrier(
        LoginCarrierRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var query = mapper.Map<LoginCarrierQuery>(request);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<AuthenticationResponse>(result));
    }
}
