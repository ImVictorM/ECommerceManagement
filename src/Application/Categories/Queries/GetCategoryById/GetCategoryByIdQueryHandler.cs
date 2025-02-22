using Application.Categories.DTOs;
using Application.Categories.Errors;
using Application.Common.Persistence;

using Domain.CategoryAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Queries.GetCategoryById;

/// <summary>
/// Handles the <see cref="GetCategoryByIdQuery"/> query.
/// </summary>
public partial class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoryByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository.</param>
    /// <param name="logger">The logger.</param>
    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, ILogger<GetCategoryByIdQueryHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CategoryResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingGetCategory(request.Id);

        var categoryId = CategoryId.Create(request.Id);

        var category = await _categoryRepository.FindByIdAsync(categoryId, cancellationToken);

        if (category == null)
        {
            LogCategoryNotFound();
            throw new CategoryNotFoundException();
        }

        LogCategoryRetrieved();
        return new CategoryResult(category.Id.ToString(), category.Name);
    }
}
