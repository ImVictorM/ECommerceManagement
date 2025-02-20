using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using Application.Common.Persistence;
using Application.Common.Errors;
using Application.Users.Errors;

using SharedKernel.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
/// Command handler for the command <see cref="UpdateUserCommand"/>.
/// Handles user registration.
/// </summary>
public sealed partial class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The logger.</param>
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

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingUserUpdate(request.UserId);

        var userToUpdateId = UserId.Create(request.UserId);
        var inputEmail = Email.Create(request.Email);

        var userToUpdate = await _userRepository.FindFirstSatisfyingAsync(
            new QueryActiveUserByIdSpecification(userToUpdateId),
            cancellationToken
        );

        if (userToUpdate == null)
        {
            LogUserToBeUpdatedNotFound();

            throw new UserNotFoundException("The user to be updated could not be found");
        }

        if (userToUpdate.Email != inputEmail)
        {
            LogEmailBeingUpdated(userToUpdate.Email.ToString(), request.Email);

            var userWithConflictingEmail = await _userRepository.FindFirstSatisfyingAsync(
                new QueryUserByEmailSpecification(inputEmail),
                cancellationToken
            );

            if (userWithConflictingEmail != null)
            {
                LogEmailConflict(request.Email);

                throw new EmailConflictException();
            }

            LogEmailAvailable(request.Email);
        }

        userToUpdate.UpdateDetails(
            name: request.Name,
            email: inputEmail,
            phone: request.Phone
        );

        LogUserUpdated();

        await _unitOfWork.SaveChangesAsync();

        LogUserUpdatedAndSavedSuccessfully();

        return Unit.Value;
    }
}
