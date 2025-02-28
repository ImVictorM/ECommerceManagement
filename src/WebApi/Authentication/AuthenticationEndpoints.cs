using Application.Authentication.Commands.RegisterCustomer;
using Application.Authentication.Queries.LoginUser;
using Application.Authentication.Queries.LoginCarrier;

using Contracts.Authentication;

using Microsoft.AspNetCore.Http.HttpResults;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.Authentication;

/// <summary>
/// Provides endpoints for the user authentication features.
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
            .WithName(nameof(RegisterCustomer))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Register Customer",
                Description =
                "Allows a new customer user to register for an account."
            });

        authenticationGroup
            .MapPost("/login/users", LoginUser)
            .WithName(nameof(LoginUser))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Login User",
                Description =
                "Allows a user to log in using their email and password." +
                " Inactive users cannot log in."
            });

        authenticationGroup
            .MapPost("/login/carriers", LoginCarrier)
            .WithName(nameof(LoginCarrier))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Login Carrier",
                Description =
                "Allows a carrier to log in using their email and password."
            });
    }

    internal async Task<Results<
        Created<AuthenticationResponse>,
        BadRequest,
        Conflict
    >> RegisterCustomer(
        RegisterCustomerRequest request,
        ISender sender,
        IMapper mapper,
        LinkGenerator linkGenerator
    )
    {
        var command = mapper.Map<RegisterCustomerCommand>(request);

        var result = await sender.Send(command);

        var authenticationResponse = mapper.Map<AuthenticationResponse>(result);

        return TypedResults.Created(
            $"/users/{authenticationResponse.Id}",
            authenticationResponse
        );
    }

    internal async Task<Results<Ok<AuthenticationResponse>, BadRequest>> LoginUser(
        LoginUserRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var query = mapper.Map<LoginUserQuery>(request);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<AuthenticationResponse>(result));
    }

    internal async Task<Results<Ok<AuthenticationResponse>, BadRequest>> LoginCarrier(
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
