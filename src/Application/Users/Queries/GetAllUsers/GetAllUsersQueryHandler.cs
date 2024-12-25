using Application.Common.Interfaces.Persistence;
using Application.Users.Common.DTOs;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetAllUsers;

/// <summary>
/// Query handler for the <see cref="GetAllUsersQuery"/> query.
/// </summary>
public sealed partial class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllUsersQueryHandler"/>.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllUsersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingUsersRetrieval();

        var users = request.IsActive == null ?
            await _unitOfWork.UserRepository.FindAllAsync()
            : await _unitOfWork.UserRepository.FindAllAsync(user => user.IsActive == request.IsActive);

        LogUsersRetrieved(request.IsActive);

        return users.Select(u => new UserResult(u));
    }
}
