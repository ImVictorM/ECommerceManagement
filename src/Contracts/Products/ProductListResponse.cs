namespace Contracts.Products;

/// <summary>
/// Defines multiple products response.
/// </summary>
/// <param name="Products">The products.</param>
public record ProductListResponse(IEnumerable<ProductResponse> Products);
