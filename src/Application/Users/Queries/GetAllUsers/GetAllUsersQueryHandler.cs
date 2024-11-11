using Application.Common.Interfaces.Persistence;
using Application.Users.Common.DTOs;
using MediatR;

namespace Application.Users.Queries.GetAllUsers;

/// <summary>
/// Query handler for the <see cref="GetAllUsersQuery"/> query.
/// </summary>
public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, UserListResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllUsersQueryHandler"/>.
    /// </summary>
    /// <param name="unitOfWork">The unity of work.</param>
    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<UserListResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = request.IsActive == null ?
            await _unitOfWork.UserRepository.FindAllAsync()
            : await _unitOfWork.UserRepository.FindAllAsync(user => user.IsActive == request.IsActive);

        return new UserListResult(users);
    }
}
