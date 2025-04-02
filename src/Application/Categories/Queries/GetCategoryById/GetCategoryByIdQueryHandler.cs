using Application.Common.Persistence.Repositories;
using Application.Categories.Errors;
using Application.Categories.DTOs.Results;

using Domain.CategoryAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Queries.GetCategoryById;

internal sealed partial class GetCategoryByIdQueryHandler
    : IRequestHandler<GetCategoryByIdQuery, CategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(
        ICategoryRepository categoryRepository,
        ILogger<GetCategoryByIdQueryHandler> logger
    )
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<CategoryResult> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCategoryRetrieval(request.Id);

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

        LogCategoryRetrievedSuccessfully();
        return CategoryResult.FromCategory(category);
    }
}
