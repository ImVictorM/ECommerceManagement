using Application.Categories.Common.Errors;
using Application.Common.Persistence;
using Domain.CategoryAggregate.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.UpdateCategory;

/// <summary>
/// Handles the <see cref="UpdateCategoryCommand"/> command.
/// </summary>
public partial class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateCategoryCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingCategoryUpdate(request.Id);

        var categoryId = CategoryId.Create(request.Id);

        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);

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
