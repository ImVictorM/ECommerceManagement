using Application.Common.DTOs;
using MediatR;

namespace Application.Categories.Commands.CreateCategory;

/// <summary>
/// Command to create a new category.
/// </summary>
/// <param name="Name">The category name.</param>
public record CreateCategoryCommand(string Name) : IRequest<CreatedResult>;
