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
public sealed class AuthenticationEndpoints : CarterModule
{
    /// <summary>
    /// Mediatr sender service.
    /// </summary>
    private readonly ISender _sender;

    /// <summary>
    /// Mapper service.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationEndpoints"/> class.
    /// </summary>
    /// <param name="sender">Mediatr sender service.</param>
    /// <param name="mapper">Mapper service.</param>
    public AuthenticationEndpoints(ISender sender, IMapper mapper) : base("/auth")
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Add the routes related to authentication.
    /// </summary>
    /// <param name="app">The application instance.</param>
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
        var command = _mapper.Map<RegisterCommand>(request);

        AuthenticationResult result = await _sender.Send(command);

        return Results.Created("", _mapper.Map<AuthenticationResponse>(result));
    }

    /// <summary>
    /// Route to authenticate a registered user.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <returns>An authentication response containing the user token.</returns>
    private async Task<IResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);

        AuthenticationResult result = await _sender.Send(query);

        return Results.Ok(_mapper.Map<AuthenticationResponse>(result));
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
