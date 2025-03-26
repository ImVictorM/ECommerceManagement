using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.Errors;
using Application.Users.Errors;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Commands.UpdateUser;

internal sealed partial class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        ILogger<UpdateUserCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingUserUpdate(request.UserId);

        var userId = UserId.Create(request.UserId);
        var emailUpdated = Email.Create(request.Email);

        var user = await _userRepository.FindFirstSatisfyingAsync(
            new QueryActiveUserByIdSpecification(userId),
            cancellationToken
        );

        if (user == null)
        {
            LogUserToBeUpdatedNotFound();

            throw new UserNotFoundException(
                "The user could not be updated because they either do not " +
                "exist or are inactive"
            )
            .WithContext("UserId", userId.ToString());
        }

        if (user.Email != emailUpdated)
        {
            LogEmailBeingUpdated(user.Email.ToString(), request.Email);

            var hasConflictingEmail = await _userRepository
                .FindFirstSatisfyingAsync(
                    new QueryUserByEmailSpecification(emailUpdated),
                    cancellationToken
                ) is not null;

            if (hasConflictingEmail)
            {
                LogEmailConflict(request.Email);

                throw new EmailConflictException();
            }

            LogEmailAvailable(request.Email);
        }

        user.UpdateDetails(
            name: request.Name,
            email: emailUpdated,
            phone: request.Phone
        );

        LogUserUpdated();

        await _unitOfWork.SaveChangesAsync();

        LogUserUpdatedAndSavedSuccessfully();

        return Unit.Value;
    }
}
