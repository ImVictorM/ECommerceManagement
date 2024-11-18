using Application.Products.Queries.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProductCategories;

/// <summary>
/// Query to get all the available product categories.
/// </summary>
public class GetProductCategoriesQuery : IRequest<ProductCategoriesResult>;
