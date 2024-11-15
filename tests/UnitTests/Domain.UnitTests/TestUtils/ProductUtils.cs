using Domain.ProductAggregate;
using Domain.ProductAggregate.Enumerations;
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
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantityAvailable">The product quantity available in inventory.</param>
    /// <param name="categories">The product categories.</param>
    /// <param name="productImagesUrl">The product image urls.</param>
    /// <param name="initialDiscounts">The product initial discounts.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product CreateProduct(
        string? name = null,
        string? description = null,
        decimal? price = null,
        int? quantityAvailable = null,
        IEnumerable<string>? categories = null,
        IEnumerable<Uri>? productImagesUrl = null,
        IEnumerable<Discount>? initialDiscounts = null
    )
    {
        return Product.Create(
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            price ?? DomainConstants.Product.Price,
            quantityAvailable ?? DomainConstants.Product.QuantityAvailable,
            categories ?? DomainConstants.Product.Categories,
            productImagesUrl ?? CreateProductImagesUrl(),
            initialDiscounts
        );
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
    public static IEnumerable<(decimal Value, Dictionary<string, string[]> ExpectedErrors)> GetInvalidInitialPriceWithCorrespondingErrors()
    {
        yield return (-15m, new Dictionary<string, string[]>
        {
            { "InitialPrice", [DomainConstants.Product.Validations.NegativeInitialPrice] }
        });
    }

    /// <summary>
    /// Gets a list of invalid initial quantities with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(int Value, Dictionary<string, string[]> ExpectedErrors)> GetInvalidInitialQuantityWithCorrespondingErrors()
    {
        yield return (-10, new Dictionary<string, string[]>
        {
            { "InitialQuantity", [DomainConstants.Product.Validations.NegativeInitialQuantity] }
        });
    }
}
