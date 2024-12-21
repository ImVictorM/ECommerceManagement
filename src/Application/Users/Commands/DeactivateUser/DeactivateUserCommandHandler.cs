using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.DeactivateUser;

/// <summary>
/// Handles user deactivation.
/// </summary>
public sealed partial class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public DeactivateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<DeactivateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingUserDeactivation(request.IdUserToDeactivate);

        var idCurrentUser = UserId.Create(request.IdCurrentUser);
        var idUserToBeDeactivated = UserId.Create(request.IdUserToDeactivate);

        var currentUser = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(idCurrentUser));
        var userToBeDeactivated = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(idUserToBeDeactivated));

        if (userToBeDeactivated == null)
        {
            LogUserDoesNotExist();

            return Unit.Value;
        }

        if (currentUser == null || !new DeactivateUserSpecification(currentUser).IsSatisfiedBy(userToBeDeactivated))
        {
            LogUserNotAllowed(request.IdCurrentUser);

            throw new UserNotAllowedException();
        }

        userToBeDeactivated.Deactivate();

        await _unitOfWork.SaveChangesAsync();

        LogDeactivationComplete();

        return Unit.Value;
    }
}
