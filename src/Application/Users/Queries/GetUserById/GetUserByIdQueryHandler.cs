using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// Query handler for the <see cref="GetUserByIdQuery"/> query.
/// </summary>
public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
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

    /// <summary>
    /// Handles a get user by id query request.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="GetUserByIdResult"/> result object.</returns>
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var id = UserId.Create(request.Id);

        var user =
            await _unitOfWork.UserRepository.FindByIdAsync(id) ??
            throw new BadRequestException(message: $"User with identifier {id} not found.");

        return new GetUserByIdResult(user);
    }
}
