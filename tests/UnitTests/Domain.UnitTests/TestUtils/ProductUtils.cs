using System.Reflection;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Enumerations;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// The product utilities.
/// </summary>
public static class ProductUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="id">The product id.</param>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantityAvailable">The product quantity available in inventory.</param>
    /// <param name="categories">The product categories.</param>
    /// <param name="productImagesUrl">The product image urls.</param>
    /// <param name="initialDiscounts">The product initial discounts.</param>
    /// <param name="active">Defines if the product should be created as an active or inactive product.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product CreateProduct(
        ProductId? id = null,
        string? name = null,
        string? description = null,
        decimal? price = null,
        int? quantityAvailable = null,
        IEnumerable<string>? categories = null,
        IEnumerable<Uri>? productImagesUrl = null,
        IEnumerable<Discount>? initialDiscounts = null,
        bool active = true
    )
    {
        var product = Product.Create(
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            price ?? DomainConstants.Product.Price,
            quantityAvailable ?? DomainConstants.Product.QuantityAvailable,
            categories ?? DomainConstants.Product.Categories,
            productImagesUrl ?? CreateProductImagesUrl(),
            initialDiscounts
        );

        if (id != null)
        {
            var idProperty = typeof(Product).GetProperty(nameof(Product.Id), BindingFlags.Instance | BindingFlags.Public);

            if (idProperty != null && idProperty.CanWrite)
            {
                idProperty.SetValue(product, id);
            }
        }

        if (!active)
        {
            product.MakeInactive();
        }

        return product;
    }

    /// <summary>
    /// Creates a list of products.
    /// </summary>
    /// <param name="count">The quantity of products to be created.</param>
    /// <param name="categories">The categories the products will have.</param>
    /// <param name="active">Defines if the products created should be active or inactive.</param>
    /// <returns>A list of products.</returns>
    public static IEnumerable<Product> CreateProducts(int count = 1, IEnumerable<string>? categories = null, bool active = true)
    {
        return Enumerable
            .Range(0, count)
            .Select(index => CreateProduct(
                name: DomainConstants.Product.ProductNameFromIndex(index),
                description: DomainConstants.Product.ProductDescriptionFromIndex(index),
                categories: categories,
                active: active
            ));
    }

    /// <summary>
    /// Creates a list of product image urls.
    /// </summary>
    /// <param name="imageCount">The quantity of product images to be created.</param>
    /// <returns>A list of product image urls.</returns>
    public static IEnumerable<Uri> CreateProductImagesUrl(int imageCount = 1)
    {
        return Enumerable
            .Range(0, imageCount)
            .Select(DomainConstants.Product.ProductImageFromIndex);
    }

    /// <summary>
    /// Creates a list of the category names.
    /// </summary>
    /// <param name="categories">The categories to extract the name from.</param>
    /// <returns>A list of category names.</returns>
    public static IEnumerable<string> GetCategoryNames(params Category[] categories)
    {
        foreach (var category in categories)
        {
            yield return category.Name;
        }
    }
}
