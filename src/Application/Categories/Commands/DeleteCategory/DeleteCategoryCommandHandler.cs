using Application.Categories.Errors;
using Application.Common.Persistence;

using Domain.CategoryAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

/// <summary>
/// Handles the <see cref="DeleteCategoryCommand"/> command.
/// </summary>
public partial class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteCategoryCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="categoryRepository">The category repository.</param>
    /// <param name="logger">The logger.</param>
    public DeleteCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        ILogger<DeleteCategoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingDeleteCategory(request.Id);
        var categoryId = CategoryId.Create(request.Id);

        var category = await _categoryRepository.FindByIdAsync(categoryId, cancellationToken);

        if (category == null)
        {
            LogCategoryNotFound();
            throw new CategoryNotFoundException();
        }

        _categoryRepository.RemoveOrDeactivate(category);

        await _unitOfWork.SaveChangesAsync();
        LogCategoryDeleted();

        return Unit.Value;
    }
}
