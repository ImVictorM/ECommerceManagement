using Domain.UserAggregate.Specification;
using Domain.UserAggregate.ValueObjects;

using Application.Common.Errors;
using Application.Common.Persistence;

using SharedKernel.Errors;
using SharedKernel.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

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
        if (request.UserId == null)
        {
            throw new EmptyArgumentException();
        }

        LogInitiatingUserUpdate(request.UserId);

        var userToUpdateId = UserId.Create(request.UserId);
        var inputEmail = Email.Create(request.Email);

        var userToUpdate = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(userToUpdateId))
            ?? throw new UserNotFoundException("The user to be updated could not be found");

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

        await _unitOfWork.SaveChangesAsync();

        LogUpdateComplete();

        return Unit.Value;
    }
}
