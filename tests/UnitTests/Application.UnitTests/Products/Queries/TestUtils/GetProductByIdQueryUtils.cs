using Application.Products.Queries.GetProductById;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Products.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetProductByIdQuery"/> query.
/// </summary>
public static class GetProductByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetProductByIdQuery"/> query.
    /// </summary>
    /// <param name="id">The product id.</param>
    /// <returns>A new instance of the <see cref="GetProductByIdQuery"/> query.</returns>
    public static GetProductByIdQuery CreateQuery(string? id = null)
    {
        return new GetProductByIdQuery(id ?? DomainConstants.Product.Id.ToString());
    }
}
