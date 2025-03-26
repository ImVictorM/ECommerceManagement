using Application.Common.Persistence.Repositories;
using Application.Common.Security.Identity;
using Application.Users.Errors;
using Application.Users.DTOs.Results;

using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Queries.GetSelf;

internal sealed partial class GetSelfQueryHandler
    : IRequestHandler<GetSelfQuery, UserResult>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly IUserRepository _userRepository;

    public GetSelfQueryHandler(
        IIdentityProvider identityProvider,
        IUserRepository userRepository,
        ILogger<GetSelfQueryHandler> logger
    )
    {
        _identityProvider = identityProvider;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserResult> Handle(
        GetSelfQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingSelfRetrieval();

        var currentUser = _identityProvider.GetCurrentUserIdentity(cancellationToken);
        var currentUserId = UserId.Create(currentUser.Id);

        LogCurrentUserId(currentUser.Id);

        var user = await _userRepository.FindByIdAsync(
            currentUserId,
            cancellationToken
        );

        if (user == null)
        {
            LogCurrentUserNotFoundInternally();
            throw new UserNotFoundException().WithContext("UserId", currentUser.Id);
        }

        LogCurrentUserInfoRetrieved();
        return UserResult.FromUser(user);
    }
}
