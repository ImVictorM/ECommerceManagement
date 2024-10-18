using Domain.Common.Errors;
using Domain.Common.Models;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.ProductAggregate.Entities;

/// <summary>
/// Represents a product category.
/// </summary>
public sealed class ProductCategory : Entity<ProductCategoryId>
{
    /// <summary>
    /// Represents the electronics category.
    /// </summary>
    public static readonly ProductCategory Electronics = new(ProductCategoryId.Create(1), nameof(Electronics).ToLowerInvariant());

    /// <summary>
    /// Represents the home appliances category.
    /// </summary>
    public static readonly ProductCategory HomeAppliances = new(ProductCategoryId.Create(2), nameof(HomeAppliances).ToLowerInvariant());

    /// <summary>
    /// Represents the fashion category.
    /// </summary>
    public static readonly ProductCategory Fashion = new(ProductCategoryId.Create(3), nameof(Fashion).ToLowerInvariant());

    /// <summary>
    /// Represents the footwear category.
    /// </summary>
    public static readonly ProductCategory Footwear = new(ProductCategoryId.Create(4), nameof(Footwear).ToLowerInvariant());

    /// <summary>
    /// Represents the beauty category.
    /// </summary>
    public static readonly ProductCategory Beauty = new(ProductCategoryId.Create(5), nameof(Beauty).ToLowerInvariant());

    /// <summary>
    /// Represents the health and wellness category.
    /// </summary>
    public static readonly ProductCategory HealthWellness = new(ProductCategoryId.Create(6), "health_wellness");

    /// <summary>
    /// Represents the groceries category.
    /// </summary>
    public static readonly ProductCategory Groceries = new(ProductCategoryId.Create(7), nameof(Groceries).ToLowerInvariant());

    /// <summary>
    /// Represents the furniture category.
    /// </summary>
    public static readonly ProductCategory Furniture = new(ProductCategoryId.Create(8), nameof(Furniture).ToLowerInvariant());

    /// <summary>
    /// Represents the toys and games category.
    /// </summary>
    public static readonly ProductCategory ToysGames = new(ProductCategoryId.Create(9), "toys_games");

    /// <summary>
    /// Represents the books and stationery category.
    /// </summary>
    public static readonly ProductCategory BooksStationery = new(ProductCategoryId.Create(10), "books_stationery");

    /// <summary>
    /// Represents the sports and outdoor category.
    /// </summary>
    public static readonly ProductCategory SportsOutdoor = new(ProductCategoryId.Create(11), "sports_outdoor");

    /// <summary>
    /// Represents the automotive category.
    /// </summary>
    public static readonly ProductCategory Automotive = new(ProductCategoryId.Create(12), nameof(Automotive).ToLowerInvariant());

    /// <summary>
    /// Represents the pet supplies category.
    /// </summary>
    public static readonly ProductCategory PetSupplies = new(ProductCategoryId.Create(13), "pet_supplies");

    /// <summary>
    /// Represents the jewelry and watches category.
    /// </summary>
    public static readonly ProductCategory JewelryWatches = new(ProductCategoryId.Create(14), "jewelry_watches");

    /// <summary>
    /// Represents the office and school supplies category.
    /// </summary>
    public static readonly ProductCategory OfficeSupplies = new(ProductCategoryId.Create(15), "office_supplies");

    /// <summary>
    /// Represents the home improvement and tools category.
    /// </summary>
    public static readonly ProductCategory HomeImprovement = new(ProductCategoryId.Create(16), "home_improvement");

    /// <summary>
    /// Represents the baby products category.
    /// </summary>
    public static readonly ProductCategory BabyProducts = new(ProductCategoryId.Create(17), "baby_products");

    /// <summary>
    /// Represents the travel and luggage category.
    /// </summary>
    public static readonly ProductCategory TravelLuggage = new(ProductCategoryId.Create(18), "travel_luggage");

    /// <summary>
    /// Represents the music and instruments category.
    /// </summary>
    public static readonly ProductCategory MusicInstruments = new(ProductCategoryId.Create(19), "music_instruments");

    /// <summary>
    /// Gets the name of the product category.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategory"/> class.
    /// </summary>
    private ProductCategory() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategory"/> class.
    /// </summary>
    /// <param name="id">The product category identifier.</param>
    /// <param name="name">The name of the product category.</param>
    private ProductCategory(ProductCategoryId id, string name) : base(id)
    {
        Name = name;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductCategory"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the product category.</param>
    /// <returns>A new instance of <see cref="ProductCategory"/> with the specified name.</returns>
    public static ProductCategory Create(string name)
    {
        return GetCategoryByName(name) ?? throw new DomainValidationException($"The {name} category does not exist");
    }

    /// <summary>
    /// Gets a product category by name, or null if not found.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>The category or null.</returns>
    private static ProductCategory? GetCategoryByName(string name)
    {
        return List().FirstOrDefault(category => category.Name == name);
    }

    /// <summary>
    /// Gets all the predefined product categories in a list format.
    /// </summary>
    /// <returns>All predefined product categories.</returns>
    public static IEnumerable<ProductCategory> List()
    {
        return
        [
            Electronics,
            HomeAppliances,
            Fashion,
            Footwear,
            Beauty,
            HealthWellness,
            Groceries,
            Furniture,
            ToysGames,
            BooksStationery,
            SportsOutdoor,
            Automotive,
            PetSupplies,
            JewelryWatches,
            OfficeSupplies,
            HomeImprovement,
            BabyProducts,
            TravelLuggage,
            MusicInstruments
        ];
    }
}
