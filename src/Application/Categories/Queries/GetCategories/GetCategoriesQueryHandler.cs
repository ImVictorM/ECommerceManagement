using Application.Categories.DTOs;
using Application.Common.Persistence;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
/// Handles the <see cref="GetCategoriesQuery"/> query.
/// </summary>
public partial class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryResult>>
{
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoriesQueryHandler"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository.</param>
    /// <param name="logger">The logger.</param>
    public GetCategoriesQueryHandler(
        ICategoryRepository categoryRepository,
        ILogger<GetCategoriesQueryHandler> logger
    )
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryResult>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        LogStartQueryingAllCategories();

        var categories = await _categoryRepository.FindAllAsync(cancellationToken: cancellationToken);

        LogAllCategoriesFetched();
        return categories.Select(c => new CategoryResult(c.Id.ToString(), c.Name));
    }
}
