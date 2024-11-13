using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Users.Common.Errors;
using Domain.UserAggregate.ValueObjects;
using MediatR;
using SharedKernel.ValueObjects;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
/// Command handler for the command <see cref="UpdateUserCommand"/>.
/// Handles user registration.
/// </summary>
public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userToUpdateId = UserId.Create(request.Id);
        var inputEmail = Email.Create(request.Email);

        var user = await
            _unitOfWork.UserRepository.FindByIdAsync(userToUpdateId) ??
            throw new UserNotFoundException("The user to be updated does not exist").WithContext("UserId", userToUpdateId.ToString());

        if (user.Email != inputEmail)
        {
            var existingUserWithEmail = await _unitOfWork.UserRepository.FindOneOrDefaultAsync(user => user.Email == inputEmail);

            if (existingUserWithEmail != null)
            {
                throw new UserAlreadyExistsException("The email you entered is already in use");
            }
        }

        user.Update(
            name: request.Name,
            email: inputEmail,
            phone: request.Phone
        );

        await _unitOfWork.UserRepository.UpdateAsync(user);

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
