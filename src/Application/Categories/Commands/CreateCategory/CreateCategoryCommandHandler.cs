using Application.Common.Persistence;
using Application.Common.DTOs.Results;
using Application.Common.Persistence.Repositories;

using Domain.CategoryAggregate;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Categories.Commands.CreateCategory;

internal sealed partial class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        ILogger<CreateCategoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<CreatedResult> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCategoryCreation(request.Name);

        var category = Category.Create(request.Name);

        await _categoryRepository.AddAsync(category);

        await _unitOfWork.SaveChangesAsync();

        LogCategoryCreatedAndSaved();

        return CreatedResult.FromId(category.Id.ToString());
    }
}
