using Application.Common.Errors;
using Application.Common.Persistence;
using Application.Users.Common.DTOs;

using Domain.UserAggregate.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// Query handler for the <see cref="GetUserByIdQuery"/> query.
/// </summary>
public sealed partial class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<UserResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingUserFetch(request.Id);

        var id = UserId.Create(request.Id);

        var user = await _unitOfWork.UserRepository.FindByIdAsync(id);

        if (user == null)
        {
            LogUserNotFound();

            throw new UserNotFoundException($"User with id {id} was not found")
                .WithContext("UserId", id.ToString());
        }

        LogUserFound();
        return new UserResult(user);
    }
}
