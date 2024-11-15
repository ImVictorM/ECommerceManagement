using Application.Common.Interfaces.Persistence;
using Application.Users.Common.DTOs;
using Application.Users.Common.Errors;
using Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// Query handler for the <see cref="GetUserByIdQuery"/> query.
/// </summary>
public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unity of work.</param>
    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<UserResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var id = UserId.Create(request.Id);

        var user =
            await _unitOfWork.UserRepository.FindByIdAsync(id) ??
            throw new UserNotFoundException($"User with id {id} was not found")
                .WithContext("UserId", id.ToString());

        return new UserResult(user);
    }
}
