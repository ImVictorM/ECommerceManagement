using SharedKernel.Errors;
using SharedKernel.Extensions;
using SharedKernel.Models;

namespace Domain.ProductAggregate.Enumerations;

/// <summary>
/// Represents a product category.
/// </summary>
public sealed class Category : BaseEnumeration
{
    /// <summary>
    /// Represents the electronics category.
    /// </summary>
    public static readonly Category Electronics = new(1, nameof(Electronics).ToLowerSnakeCase());

    /// <summary>
    /// Represents the home appliances category.
    /// </summary>
    public static readonly Category HomeAppliances = new(2, nameof(HomeAppliances).ToLowerSnakeCase());

    /// <summary>
    /// Represents the fashion category.
    /// </summary>
    public static readonly Category Fashion = new(3, nameof(Fashion).ToLowerSnakeCase());

    /// <summary>
    /// Represents the footwear category.
    /// </summary>
    public static readonly Category Footwear = new(4, nameof(Footwear).ToLowerSnakeCase());

    /// <summary>
    /// Represents the beauty category.
    /// </summary>
    public static readonly Category Beauty = new(5, nameof(Beauty).ToLowerSnakeCase());

    /// <summary>
    /// Represents the health and wellness category.
    /// </summary>
    public static readonly Category HealthWellness = new(6, nameof(HealthWellness).ToLowerSnakeCase());

    /// <summary>
    /// Represents the groceries category.
    /// </summary>
    public static readonly Category Groceries = new(7, nameof(Groceries).ToLowerSnakeCase());

    /// <summary>
    /// Represents the furniture category.
    /// </summary>
    public static readonly Category Furniture = new(8, nameof(Furniture).ToLowerSnakeCase());

    /// <summary>
    /// Represents the toys and games category.
    /// </summary>
    public static readonly Category ToysGames = new(9, nameof(ToysGames).ToLowerSnakeCase());

    /// <summary>
    /// Represents the books and stationery category.
    /// </summary>
    public static readonly Category BooksStationery = new(10, nameof(BooksStationery).ToLowerSnakeCase());

    /// <summary>
    /// Represents the sports and outdoor category.
    /// </summary>
    public static readonly Category SportsOutdoor = new(11, nameof(SportsOutdoor).ToLowerSnakeCase());

    /// <summary>
    /// Represents the automotive category.
    /// </summary>
    public static readonly Category Automotive = new(12, nameof(Automotive).ToLowerSnakeCase());

    /// <summary>
    /// Represents the pet supplies category.
    /// </summary>
    public static readonly Category PetSupplies = new(13, nameof(PetSupplies).ToLowerSnakeCase());

    /// <summary>
    /// Represents the jewelry and watches category.
    /// </summary>
    public static readonly Category JewelryWatches = new(14, nameof(JewelryWatches).ToLowerSnakeCase());

    /// <summary>
    /// Represents the office and school supplies category.
    /// </summary>
    public static readonly Category OfficeSupplies = new(15, nameof(OfficeSupplies).ToLowerSnakeCase());

    /// <summary>
    /// Represents the home improvement and tools category.
    /// </summary>
    public static readonly Category HomeImprovement = new(16, nameof(HomeImprovement).ToLowerSnakeCase());

    /// <summary>
    /// Represents the baby products category.
    /// </summary>
    public static readonly Category BabyProducts = new(17, nameof(BabyProducts).ToLowerSnakeCase());

    /// <summary>
    /// Represents the travel and luggage category.
    /// </summary>
    public static readonly Category TravelLuggage = new(18, nameof(TravelLuggage).ToLowerSnakeCase());

    /// <summary>
    /// Represents the music and instruments category.
    /// </summary>
    public static readonly Category MusicInstruments = new(19, nameof(MusicInstruments).ToLowerSnakeCase());

    /// <summary>
    /// Initializes a new instance of the <see cref="Category"/> class.
    /// </summary>
    private Category() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Category"/> class.
    /// </summary>
    /// <param name="id">The product category identifier.</param>
    /// <param name="name">The name of the product category.</param>
    private Category(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Category"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the product category.</param>
    /// <returns>A new instance of <see cref="Category"/> with the specified name.</returns>
    /// <exception cref="DomainValidationException">Thrown when category of the specified name does not exist.</exception>
    public static Category Create(string name)
    {
        return GetCategoryByName(name) ?? throw new DomainValidationException($"The {name} category does not exist");
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Category"/> class with the specified id.
    /// </summary>
    /// <param name="id">The category id.</param>
    /// <returns>A new instance of the <see cref="Category"/> class.</returns>
    /// <exception cref="DomainValidationException">Thrown when category of the specified id does not exist.</exception>
    public static Category Create(long id)
    {
        return GetCategoryById(id) ?? throw new DomainValidationException($"Category with id {id} does not exist");
    }

    /// <summary>
    /// Gets all the predefined product categories in a list format.
    /// </summary>
    /// <returns>All predefined product categories.</returns>
    public static IEnumerable<Category> List()
    {
        return GetAll<Category>().ToList();
    }

    private static Category? GetCategoryById(long id)
    {
        return List().FirstOrDefault(c => c.Id == id);
    }

    private static Category? GetCategoryByName(string name)
    {
        return List().FirstOrDefault(category => category.Name == name);
    }
}
