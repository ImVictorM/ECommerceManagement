using Domain.ProductCategoryAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// The product category utilities.
/// </summary>
public static class ProductCategoryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="ProductCategoryId"/> class.
    /// </summary>
    /// <param name="id">The category id.</param>
    /// <returns>A new instance of the <see cref="ProductCategoryId"/> class.</returns>
    public static ProductCategoryId CreateProductCategoryId(long? id = null)
    {
        return ProductCategoryId.Create(id ?? TestConstants.ProductCategory.Id);
    }
}
