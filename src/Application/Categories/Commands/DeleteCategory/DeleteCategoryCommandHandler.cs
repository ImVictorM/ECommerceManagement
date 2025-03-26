using Application.Categories.Errors;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.CategoryAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

internal sealed partial class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

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

    public async Task<Unit> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCategoryDeletion(request.Id);

        var categoryId = CategoryId.Create(request.Id);

        var category = await _categoryRepository.FindByIdAsync(
            categoryId,
            cancellationToken
        );

        if (category == null)
        {
            LogCategoryNotFound();
            throw new CategoryNotFoundException();
        }

        _categoryRepository.RemoveOrDeactivate(category);

        await _unitOfWork.SaveChangesAsync();

        LogCategoryDeletedSuccessfully();

        return Unit.Value;
    }
}
