using Application.Users.Common;
using MediatR;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// Query to get the user by identifier.
/// </summary>
/// <param name="Id">The identifier value.</param>
public record GetUserByIdQuery(string Id) : IRequest<UserResult>;
