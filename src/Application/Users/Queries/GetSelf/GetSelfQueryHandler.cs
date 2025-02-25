using Application.Common.Persistence.Repositories;
using Application.Common.Security.Identity;
using Application.Users.DTOs;
using Application.Users.Errors;

using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Queries.GetSelf;

internal sealed partial class GetSelfQueryHandler : IRequestHandler<GetSelfQuery, UserResult>
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

    /// <inheritdoc/>
    public async Task<UserResult> Handle(GetSelfQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingSelfRetrieval();

        var currentUser = _identityProvider.GetCurrentUserIdentity();

        LogCurrentUserId(currentUser.Id);

        var currentUserId = UserId.Create(currentUser.Id);

        var user = await _userRepository.FindByIdAsync(currentUserId, cancellationToken);

        if (user == null)
        {
            LogCurrentUserNotFoundInternally();
            throw new UserNotFoundException($"User with id {currentUserId} was not found");
        }

        LogCurrentUserInfoRetrieved();
        return new UserResult(user);
    }
}
