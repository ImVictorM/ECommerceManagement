using Application.Categories.Common.DTOs;
using MediatR;

namespace Application.Categories.Queries.GetCategoryById;

/// <summary>
/// Represents a query to retrieve a category by id.
/// </summary>
/// <param name="Id">The category id.</param>
public record GetCategoryByIdQuery(string Id) : IRequest<CategoryResult>;
