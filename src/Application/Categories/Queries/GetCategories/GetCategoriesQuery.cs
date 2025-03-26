using Application.Categories.DTOs.Results;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
/// Represents a query to list all available categories.
/// </summary>
public class GetCategoriesQuery : IRequest<IReadOnlyList<CategoryResult>>;
