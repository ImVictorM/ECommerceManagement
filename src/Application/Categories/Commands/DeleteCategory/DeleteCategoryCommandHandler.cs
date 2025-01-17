using Application.Categories.Common.Errors;
using Application.Common.Persistence;
using Domain.CategoryAggregate.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.DeleteCategory;

/// <summary>
/// Handles the <see cref="DeleteCategoryCommand"/> command.
/// </summary>
public partial class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteCategoryCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingDeleteCategory(request.Id);
        var categoryId = CategoryId.Create(request.Id);

        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);

        if (category == null)
        {
            LogCategoryNotFound();
            throw new CategoryNotFoundException();
        }

        _unitOfWork.CategoryRepository.RemoveOrDeactivate(category);

        await _unitOfWork.SaveChangesAsync();
        LogCategoryDeleted();

        return Unit.Value;
    }
}
