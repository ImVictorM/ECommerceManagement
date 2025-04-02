using Application.Common.Persistence.Repositories;
using Application.Categories.DTOs.Results;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

internal sealed partial class GetCategoriesQueryHandler
    : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryResult>>
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

    public async Task<IReadOnlyList<CategoryResult>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCategoriesRetrieval();

        var categories = await _categoryRepository.FindAllAsync(
            cancellationToken: cancellationToken
        );

        var result = categories
            .Select(CategoryResult.FromCategory)
            .ToList();

        LogCategoriesRetrievedSuccessfully(result.Count);

        return result;
    }
}
