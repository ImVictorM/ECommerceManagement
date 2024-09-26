using Application.Authentication.Commands.Register;
using Application.Authentication.Common;
using Application.Authentication.Queries.Login;
using Carter;
using Contracts.Authentication;
using MapsterMapper;
using MediatR;

namespace WebApi.Endpoints;

/// <summary>
/// Wraps the routes related to authentication.
/// </summary>
public sealed class AuthenticationEndpoints : ICarterModule
{
    /// <summary>
    /// Add the routes related to authentication.
    /// </summary>
    /// <param name="app">The application instance.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var authenticationGroup = app.MapGroup("/auth");

        authenticationGroup.MapPost("/register", Register);
        authenticationGroup.MapPost("/login", Login);
    }

    /// <summary>
    /// Route to register a new user.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="mapper">The mapper used to map objects.</param>
    /// <param name="sender">The sender used to send command/queries.</param>
    /// <returns>An authentication response containing the token.</returns>
    private async Task<IResult> Register(
        RegisterRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<RegisterCommand>(request);

        AuthenticationResult result = await sender.Send(command);

        return Results.Created("", mapper.Map<AuthenticationResponse>(result));
    }

    /// <summary>
    /// Route to authenticate a registered user.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="mapper">The mapper used to map objects.</param>
    /// <param name="sender">The sender used to send command/queries.</param>
    /// <returns>An authentication response containing the user token.</returns>
    private async Task<IResult> Login(
        LoginRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var query = mapper.Map<LoginQuery>(request);

        AuthenticationResult result = await sender.Send(query);

        return Results.Ok(mapper.Map<AuthenticationResponse>(result));
    }
}
