using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Users.Errors;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Commands.DeactivateUser;

internal sealed partial class DeactivateUserCommandHandler
    : IRequestHandler<DeactivateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

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

    public async Task<Unit> Handle(
        DeactivateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingUserDeactivation(request.UserId);

        var userId = UserId.Create(request.UserId);

        var userToBeDeactivated = await _userRepository.FindFirstSatisfyingAsync(
            new QueryActiveUserByIdSpecification(userId),
            cancellationToken
        );

        if (userToBeDeactivated == null)
        {
            LogUserNotFound();
            throw new UserNotFoundException(
                    "The user could not be deactivated because they either do not" +
                    " exist or are already inactive"
                )
                .WithContext("UserId", userId.ToString());
        }

        userToBeDeactivated.Deactivate();

        await _unitOfWork.SaveChangesAsync();

        LogDeactivationComplete();

        return Unit.Value;
    }
}
