using MediatR;

namespace Application.Common.Security.Authorization.Requests;

/// <summary>
/// Represents a request that needs authorization.
/// </summary>
/// <typeparam name="T">The request return type.</typeparam>
/// <param name="UserId">The user id for requests related to users.</param>
public record RequestWithAuthorization<T>(string? UserId = null) : IRequest<T>;
