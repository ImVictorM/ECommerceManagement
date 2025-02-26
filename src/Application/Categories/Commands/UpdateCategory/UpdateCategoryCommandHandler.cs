using Application.Categories.Errors;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.CategoryAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Commands.UpdateCategory;

internal sealed partial class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        ILogger<UpdateCategoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingCategoryUpdate(request.Id);

        var categoryId = CategoryId.Create(request.Id);

        var category = await _categoryRepository.FindByIdAsync(categoryId, cancellationToken);

        if (category == null)
        {
            LogCategoryNotFound();
            throw new CategoryNotFoundException($"The category with id {categoryId} could not be updated because it does not exist");
        }

        category.Update(request.Name);

        await _unitOfWork.SaveChangesAsync();
        LogCategoryUpdated();

        return Unit.Value;
    }
}
