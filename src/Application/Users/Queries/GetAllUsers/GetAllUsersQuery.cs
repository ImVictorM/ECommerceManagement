using Application.Users.Common.DTOs;
using MediatR;

namespace Application.Users.Queries.GetAllUsers;

/// <summary>
/// Query for all users.
/// </summary>
/// <param name="IsActive">Filter condition for querying the users that is active or not (optional).</param>
public record GetAllUsersQuery(bool? IsActive = null) : IRequest<UserListResult>;
