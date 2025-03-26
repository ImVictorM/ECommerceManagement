using Application.Users.DTOs.Results;

using MediatR;

namespace Application.Users.Queries.GetSelf;

/// <summary>
/// Represents a query to retrieve the current user details.
/// </summary>
public class GetSelfQuery : IRequest<UserResult>;
