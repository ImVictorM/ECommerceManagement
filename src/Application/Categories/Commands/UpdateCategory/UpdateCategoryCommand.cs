using MediatR;

namespace Application.Categories.Commands.UpdateCategory;

/// <summary>
/// Command to update a category.
/// </summary>
/// <param name="Id">The category id.</param>
/// <param name="Name">The new category name.</param>
public record UpdateCategoryCommand(string Id, string Name) : IRequest<Unit>;
