using Application.Common.Persistence.Repositories;
using Application.Users.DTOs.Results;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Queries.GetUsers;

internal sealed partial class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, IReadOnlyList<UserResult>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(
        IUserRepository userRepository,
        ILogger<GetUsersQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<UserResult>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingUsersRetrieval(request.Filters.IsActive);

        var users = await _userRepository.GetUsersAsync(
            request.Filters,
            cancellationToken
        );

        LogUsersRetrievedSuccessfully();

        return users
            .Select(UserResult.FromUser)
            .ToList();
    }
}
