using Application.Common.Interfaces.Persistence;
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
        LogInitiatingUserDeactivation(request.Id);

        var user = await _unitOfWork.UserRepository.FindByIdAsync(UserId.Create(request.Id));

        if (user == null)
        {
            LogUserDoesNotExist();

            return Unit.Value;
        }

        user.MakeInactive();

        await _unitOfWork.SaveChangesAsync();

        LogDeactivationComplete();

        return Unit.Value;
    }
}
