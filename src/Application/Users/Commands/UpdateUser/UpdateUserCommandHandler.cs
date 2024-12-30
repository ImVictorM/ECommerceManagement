using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.ValueObjects;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
/// Command handler for the command <see cref="UpdateUserCommand"/>.
/// Handles user registration.
/// </summary>
public sealed partial class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingUserUpdate(request.IdUserToUpdate);

        var currentUserId = UserId.Create(request.IdCurrentUser);
        var userToUpdateId = UserId.Create(request.IdUserToUpdate);
        var inputEmail = Email.Create(request.Email);

        var currentUser =
            await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(currentUserId))
            ?? throw new UserNotFoundException("The current user could not be found");

        var userToUpdate = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(userToUpdateId))
            ?? throw new UserNotFoundException($"The user to be updated could not be found");

        if (!new UpdateUserSpecification(currentUser).IsSatisfiedBy(userToUpdate))
        {
            LogUserNotAllowed();
            throw new UserNotAllowedException($"The current user is not allowed to update the user with id {userToUpdate.Id}");
        }

        if (userToUpdate.Email != inputEmail)
        {
            var existingUserWithEmail = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryUserByEmailSpecification(inputEmail));

            if (existingUserWithEmail != null)
            {
                LogEmailConflict();

                throw new UserAlreadyExistsException("The email you entered is already in use");
            }
        }

        LogUpdatingUser();

        userToUpdate.Update(
            name: request.Name,
            email: inputEmail,
            phone: request.Phone
        );

        await _unitOfWork.UserRepository.UpdateAsync(userToUpdate);

        await _unitOfWork.SaveChangesAsync();

        LogUpdateComplete();

        return Unit.Value;
    }
}
