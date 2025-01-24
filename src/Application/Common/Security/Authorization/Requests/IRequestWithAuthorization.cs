using MediatR;

namespace Application.Common.Security.Authorization.Requests;

/// <summary>
/// Represents a request that needs authorization.
/// </summary>
/// <typeparam name="T">The request return type.</typeparam>
public interface IRequestWithAuthorization<out T> : IRequest<T>
{
    /// <summary>
    /// Gets the user id for user specific requests.
    /// </summary>
    public string? UserId { get; }
}
