using Application.Categories.DTOs;

using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
/// Query to list all categories.
/// </summary>
public class GetCategoriesQuery : IRequest<IEnumerable<CategoryResult>>;
