using Application.Categories.DTOs.Results;

using MediatR;

namespace Application.Categories.Queries.GetCategoryById;

/// <summary>
/// Represents a query to retrieve a category by its identifier.
/// </summary>
/// <param name="Id">The category identifier.</param>
public record GetCategoryByIdQuery(string Id) : IRequest<CategoryResult>;
