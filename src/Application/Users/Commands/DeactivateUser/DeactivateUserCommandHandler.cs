using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Application.Users.Commands.DeactivateUser;

/// <summary>
/// Handles user deactivation.
/// </summary>
public sealed class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unity of work.</param>
    public DeactivateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {

        var user = await _unitOfWork.UserRepository.FindByIdAsync(UserId.Create(request.Id));

        if (user == null)
        {
            return Unit.Value;
        }

        user.MakeInactive();

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
