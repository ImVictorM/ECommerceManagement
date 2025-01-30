using Application.Common.Persistence;
using Application.Common.Security.Identity;
using Application.Users.DTOs;
using Application.Users.Errors;

using Domain.UserAggregate.ValueObjects;

using MediatR;

namespace Application.Users.Queries.GetSelf;

/// <summary>
/// Handles the <see cref="GetSelfQuery"/> query.
/// </summary>
public sealed class GetSelfQueryHandler : IRequestHandler<GetSelfQuery, UserResult>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetSelfQueryHandler"/> class.
    /// </summary>
    /// <param name="identityProvider">The identity provider.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public GetSelfQueryHandler(IIdentityProvider identityProvider, IUnitOfWork unitOfWork)
    {
        _identityProvider = identityProvider;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<UserResult> Handle(GetSelfQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _identityProvider.GetCurrentUserIdentity();
        var currentUserId = UserId.Create(currentUser.Id);

        var user = await _unitOfWork.UserRepository.FindByIdAsync(currentUserId)
            ?? throw new UserNotFoundException($"User with id {currentUserId} was not found");

        return new UserResult(user);
    }
}
