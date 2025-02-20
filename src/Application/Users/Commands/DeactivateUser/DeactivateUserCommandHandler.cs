using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using Application.Common.Persistence;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Commands.DeactivateUser;

/// <summary>
/// Handles user deactivation.
/// </summary>
public sealed partial class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The logger.</param>
    public DeactivateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        ILogger<DeactivateUserCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingUserDeactivation(request.UserId);

        var idUserToBeDeactivated = UserId.Create(request.UserId);

        var userToBeDeactivated = await _userRepository.FindFirstSatisfyingAsync(
            new QueryActiveUserByIdSpecification(idUserToBeDeactivated),
            cancellationToken
        );

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
