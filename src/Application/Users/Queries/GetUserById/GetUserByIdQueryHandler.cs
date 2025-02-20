using Application.Common.Persistence;
using Application.Users.DTOs;
using Application.Users.Errors;

using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// Query handler for the <see cref="GetUserByIdQuery"/> query.
/// </summary>
public sealed partial class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResult>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The logger.</param>
    public GetUserByIdQueryHandler(IUserRepository userRepository, ILogger<GetUserByIdQueryHandler> logger)
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
