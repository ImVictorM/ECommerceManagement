using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using Application.Common.Persistence;

using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Errors;

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
        if (request.UserId == null)
        {
            throw new EmptyArgumentException();
        }

        LogInitiatingUserDeactivation(request.UserId);

        var idUserToBeDeactivated = UserId.Create(request.UserId);

        var userToBeDeactivated = await _unitOfWork.UserRepository
            .FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(idUserToBeDeactivated));

        if (userToBeDeactivated == null)
        {
            LogUserDoesNotExist();

            return Unit.Value;
        }

        userToBeDeactivated.Deactivate();

        await _unitOfWork.SaveChangesAsync();

        LogDeactivationComplete();

        return Unit.Value;
    }
}
