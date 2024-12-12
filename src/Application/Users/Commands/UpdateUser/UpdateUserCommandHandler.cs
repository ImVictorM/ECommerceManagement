using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Users.Common.Errors;
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
        LogInitiatingUserUpdate(request.Id);

        var userToUpdateId = UserId.Create(request.Id);
        var inputEmail = Email.Create(request.Email);

        var user = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(userToUpdateId));

        if (user == null)
        {
            LogUserNotFound();
            throw new UserNotFoundException("The user to be updated does not exist").WithContext("UserId", userToUpdateId.ToString());
        }
            
        if (user.Email != inputEmail)
        {
            var existingUserWithEmail = await _unitOfWork.UserRepository.FindFirstSatisfyingAsync(new QueryUserByEmailSpecification(inputEmail));

            if (existingUserWithEmail != null)
            {
                LogEmailConflict();

                throw new UserAlreadyExistsException("The email you entered is already in use");
            }
        }

        LogUpdatingUser();

        user.Update(
            name: request.Name,
            email: inputEmail,
            phone: request.Phone
        );

        await _unitOfWork.UserRepository.UpdateAsync(user);

        await _unitOfWork.SaveChangesAsync();

        LogUpdateComplete();

        return Unit.Value;
    }
}
