using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

/// <summary>
/// Command to delete a category.
/// </summary>
/// <param name="Id">The category id.</param>
public record DeleteCategoryCommand(string Id) : IRequest<Unit>;
