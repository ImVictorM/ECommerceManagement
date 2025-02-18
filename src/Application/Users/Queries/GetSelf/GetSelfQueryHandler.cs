using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Users.DTOs;
using Application.Users.Errors;

using Domain.UserAggregate.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetSelf;

/// <summary>
/// Handles the <see cref="GetSelfQuery"/> query.
/// </summary>
public sealed partial class GetSelfQueryHandler : IRequestHandler<GetSelfQuery, UserResult>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetSelfQueryHandler"/> class.
    /// </summary>
    /// <param name="identityProvider">The identity provider.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetSelfQueryHandler(
        IIdentityProvider identityProvider,
        IUnitOfWork unitOfWork,
        ILogger<GetSelfQueryHandler> logger
    )
    {
        _identityProvider = identityProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<UserResult> Handle(GetSelfQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingSelfRetrieval();

        var currentUser = _identityProvider.GetCurrentUserIdentity();

        LogCurrentUserId(currentUser.Id);

        var currentUserId = UserId.Create(currentUser.Id);

        var user = await _unitOfWork.UserRepository.FindByIdAsync(currentUserId);

        if (user == null)
        {
            LogCurrentUserNotFoundInternally();
            throw new UserNotFoundException($"User with id {currentUserId} was not found");
        }

        LogCurrentUserInfoRetrieved();
        return new UserResult(user);
    }
}
