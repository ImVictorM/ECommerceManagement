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

    /// <summary>
    /// Gets a list of invalid names with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(string Value, Dictionary<string, string[]> ExpectedErrors)> GetInvalidNameWithCorrespondingErrors()
    {
        yield return ("", new Dictionary<string, string[]>
        {
            { "Name", [DomainConstants.Product.Validations.EmptyName] }
        });
    }

    /// <summary>
    /// Gets a list of invalid descriptions with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(string Value, Dictionary<string, string[]> ExpectedErrors)> GetInvalidDescriptionWithCorrespondingErrors()
    {
        yield return ("", new Dictionary<string, string[]>
        {
            { "Description", [DomainConstants.Product.Validations.EmptyDescription] }
        });
    }

    /// <summary>
    /// Gets a list of invalid categories with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(IEnumerable<string>, Dictionary<string, string[]> ExpectedErrors)> GetInvalidCategoriesWithCorrespondingErrors()
    {
        yield return (new List<string>() { }, new Dictionary<string, string[]>
        {
            { "Categories", [DomainConstants.Product.Validations.EmptyCategories] }
        });
    }

    /// <summary>
    /// Gets a list of invalid images with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(IEnumerable<Uri>, Dictionary<string, string[]> ExpectedErrors)> GetInvalidImagesWithCorrespondingErrors()
    {
        yield return (new List<Uri>() { }, new Dictionary<string, string[]>
        {
            { "Images", [DomainConstants.Product.Validations.EmptyImages] }
        });
    }

    /// <summary>
    /// Gets a list of invalid initial prices with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(decimal Value, Dictionary<string, string[]> ExpectedErrors)> GetInvalidBasePriceWithCorrespondingErrors()
    {
        yield return (-15m, new Dictionary<string, string[]>
        {
            { "BasePrice", [DomainConstants.Product.Validations.LessThanZeroBasePrice] }
        });
    }

    /// <summary>
    /// Gets a list of invalid initial quantities with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(int Value, Dictionary<string, string[]> ExpectedErrors)> GetInvalidInitialQuantityWithCorrespondingErrors()
    {
        yield return (-10, new Dictionary<string, string[]>
        {
            { "InitialQuantity", [DomainConstants.Product.Validations.LessThanZeroInitialQuantity] }
        });
    }

    /// <summary>
    /// Gets a list of invalid quantities to increment with the corresponding errors similar to the validation problem details object.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<(int Value, Dictionary<string, string[]> ExpectedErrors)> GetInvalidQuantityToIncrementWithCorrespondingErrors()
    {
        yield return (-22, new Dictionary<string, string[]>
        {
            { "QuantityToIncrement", [DomainConstants.Product.Validations.LessThanZeroQuantityToIncrement] }
        });

        yield return (0, new Dictionary<string, string[]>
        {
            { "QuantityToIncrement", [DomainConstants.Product.Validations.LessThanZeroQuantityToIncrement] }
        });
    }
}
