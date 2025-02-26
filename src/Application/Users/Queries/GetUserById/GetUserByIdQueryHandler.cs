using Application.Common.Persistence.Repositories;
using Application.Users.DTOs;
using Application.Users.Errors;

using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

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

    /// <inheritdoc/>
    public async Task<UserResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingUserRetrieval(request.UserId);

        var id = UserId.Create(request.UserId);

        var user = await _userRepository.FindByIdAsync(id, cancellationToken);

        if (user == null)
        {
            LogUserNotFound();

            throw new UserNotFoundException($"User with id {id} was not found")
                .WithContext("UserId", id.ToString());
        }

        LogUserRetrievedSuccessfully();
        return new UserResult(user);
    }
}
