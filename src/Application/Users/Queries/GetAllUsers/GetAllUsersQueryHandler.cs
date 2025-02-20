using Application.Common.Persistence;
using Application.Users.DTOs;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Queries.GetAllUsers;

/// <summary>
/// Query handler for the <see cref="GetAllUsersQuery"/> query.
/// </summary>
public sealed partial class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResult>>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetAllUsersQueryHandler"/>.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The logger.</param>
    public GetAllUsersQueryHandler(IUserRepository userRepository, ILogger<GetAllUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingUsersRetrieval();

        var isActiveFilter = request.IsActive == null ? "none" : request.IsActive.ToString()!;

        LogActiveFilter(isActiveFilter);

        var users = request.IsActive == null ?
            await _userRepository.FindAllAsync(cancellationToken: cancellationToken)
            : await _userRepository.FindAllAsync(user => user.IsActive == request.IsActive, cancellationToken);

        LogUsersRetrieved();

        return users.Select(u => new UserResult(u));
    }
}
