using Application.Common.Persistence.Repositories;
using Application.Users.Errors;

using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.Users.DTOs.Results;

namespace Application.Users.Queries.GetUserById;

internal sealed partial class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, UserResult>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        ILogger<GetUserByIdQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserResult> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingUserRetrieval(request.UserId);

        var userId = UserId.Create(request.UserId);

        var user = await _userRepository.FindByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            LogUserNotFound();

            throw new UserNotFoundException()
                .WithContext("UserId", userId.ToString());
        }

        LogUserRetrievedSuccessfully();
        return UserResult.FromUser(user);
    }
}
