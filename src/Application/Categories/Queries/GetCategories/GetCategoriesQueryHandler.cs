using Application.Common.Persistence.Repositories;
using Application.Categories.DTOs;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

internal sealed partial class GetCategoriesQueryHandler
    : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryResult>>
{
    private readonly ICategoryRepository _categoryRepository;

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
