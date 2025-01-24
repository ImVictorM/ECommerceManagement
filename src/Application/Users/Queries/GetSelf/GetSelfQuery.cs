using Application.Users.Common.DTOs;

using MediatR;

namespace Application.Users.Queries.GetSelf;

/// <summary>
/// Represents a query to get the current user details.
/// Useful to get the user details when the user is authenticated.
/// </summary>
public class GetSelfQuery : IRequest<UserResult>;
