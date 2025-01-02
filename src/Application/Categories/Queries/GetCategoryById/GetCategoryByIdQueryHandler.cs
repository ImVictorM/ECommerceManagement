using Application.Categories.Common.DTOs;
using Application.Categories.Common.Errors;
using Application.Common.Interfaces.Persistence;

using Domain.CategoryAggregate.ValueObjects;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Queries.GetCategoryById;

/// <summary>
/// Handles the <see cref="GetCategoryByIdQuery"/> query.
/// </summary>
public partial class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoryByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCategoryByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CategoryResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiatingGetCategory(request.Id);

        var categoryId = CategoryId.Create(request.Id);

        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);

        if (category == null)
        {
            LogCategoryNotFound();
            throw new CategoryNotFoundException();
        }

        LogCategoryRetrieved();
        return new CategoryResult(category.Id.ToString(), category.Name);
    }
}
