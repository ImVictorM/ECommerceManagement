namespace Contracts.Products;

/// <summary>
/// Represents the product categories response.
/// </summary>
/// <param name="Categories">The category names.</param>
public record ProductCategoriesResponse(IEnumerable<string> Categories);
