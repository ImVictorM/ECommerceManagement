using Application.Common.DTOs;
using Application.Common.Interfaces.Persistence;

using Domain.CategoryAggregate;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.CreateCategory;

/// <summary>
/// Handles the <see cref="CreateCategoryCommand"/> command.
/// </summary>
public partial class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateCategoryCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateCategoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        LogCreatingCategory(request.Name);
        var category = Category.Create(request.Name);

        await _unitOfWork.CategoryRepository.AddAsync(category);

        await _unitOfWork.SaveChangesAsync();
        LogCategoryCreatedAndSaved();

        return new CreatedResult(category.Id.ToString());
    }
}
