using Application.Common.Interfaces.Persistence;
using Application.Users.Common.Errors;
using Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
/// Command handler for the command <see cref="UpdateUserCommand"/>.
/// Handles user registration.
/// </summary>
public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
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

    /// <summary>
    /// Handles the updating of a user based on the provided command.
    /// </summary>
    /// <param name="request">The <see cref="UpdateUserCommand"/> containing the details of the user to be updated.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="UserNotFoundException">Thrown when the user to be updated does not exist.</exception>
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(request.Id);

        var user = await
            _unitOfWork.UserRepository.FindByIdAsync(userId) ??
            throw new UserNotFoundException().WithContext("UserId", userId.ToString());

        user.Update(
            name: request.Name,
            email: request.Email,
            phone: request.Phone
        );

        await _unitOfWork.UserRepository.UpdateAsync(user);

        await _unitOfWork.SaveChangesAsync();
    }
}
