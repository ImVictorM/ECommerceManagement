using Application.Categories.DTOs;
using Application.Common.Persistence;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
/// Handles the <see cref="GetCategoriesQuery"/> query.
/// </summary>
public partial class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoriesQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCategoriesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryResult>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        LogStartQueryingAllCategories();

        var categories = await _unitOfWork.CategoryRepository.FindAllAsync();

        LogAllCategoriesFetched();
        return categories.Select(c => new CategoryResult(c.Id.ToString(), c.Name));
    }
}
