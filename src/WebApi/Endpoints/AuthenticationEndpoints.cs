using Application.Authentication.Commands.Register;
using Application.Authentication.Common;
using Carter;
using Contracts.Authentication;
using MediatR;

namespace WebApi.Endpoints;

/// <summary>
/// Wraps the routes related to authentication.
/// </summary>
public sealed class AuthenticationEndpoints : CarterModule
{
    /// <summary>
    /// Mediatr sender service.
    /// </summary>
    private readonly ISender _sender;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationEndpoints"/> class.
    /// </summary>
    /// <param name="sender">Mediatr sender service.</param>
    public AuthenticationEndpoints(ISender sender) : base("/auth")
    {
        _sender = sender;
    }

    /// <inheritdoc/>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/register", Register);
        app.MapPost("/login", Login);
        app.MapPost("/self", AuthByToken);
    }

    /// <summary>
    /// Route to register a new user.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <returns>An authentication response containing the token.</returns>
    private async Task<IResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.Name, request.Email, request.Password);

        AuthenticationResult result = await _sender.Send(command);

        return Results.Created("", result);
    }

    /// <summary>
    /// Route to authenticate a registered user.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <returns>An authentication response containing the user token.</returns>
    private IResult Login(LoginRequest request)
    {
        // endpoint to authenticate a user with credentials
        return Results.Ok(request);
    }

    /// <summary>
    /// Route to authenticate an user using their authentication token.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <returns>An authentication response containing the user token.</returns>
    private IResult AuthByToken(AuthByTokenRequest request)
    {
        // endpoint to authenticate a user with a token
        return Results.Ok(request);
    }
}
